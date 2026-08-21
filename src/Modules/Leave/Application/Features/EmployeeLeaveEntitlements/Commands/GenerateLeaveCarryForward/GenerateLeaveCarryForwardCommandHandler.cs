using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.GenerateLeaveCarryForward;

public sealed class GenerateLeaveCarryForwardCommandHandler
    : IRequestHandler<GenerateLeaveCarryForwardCommand>
{
    private readonly IReadRepository<LeaveYear, Guid> _leaveYearRepository;

    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid>
        _entitlementReadRepository;

    private readonly IReadRepository<LeavePolicyRule, Guid>
        _policyRuleRepository;

    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid>
        _entitlementWriteRepository;

    private readonly IUnitOfWorkTransaction _transaction;

    public GenerateLeaveCarryForwardCommandHandler(
        IReadRepository<LeaveYear, Guid> leaveYearRepository,
        IReadRepository<EmployeeLeaveEntitlement, Guid> entitlementReadRepository,
        IReadRepository<LeavePolicyRule, Guid> policyRuleRepository,
        IWriteRepository<EmployeeLeaveEntitlement, Guid> entitlementWriteRepository,
        IUnitOfWorkTransaction transaction)
    {
        _leaveYearRepository = leaveYearRepository;
        _entitlementReadRepository = entitlementReadRepository;
        _policyRuleRepository = policyRuleRepository;
        _entitlementWriteRepository = entitlementWriteRepository;
        _transaction = transaction;
    }

    public async Task Handle(
        GenerateLeaveCarryForwardCommand request,
        CancellationToken cancellationToken)
    {
        await _transaction.BeginAsync(cancellationToken);

        try
        {
            // 1. Get target leave year
            var targetLeaveYear =
                await _leaveYearRepository.GetByIdAsync(
                    request.LeaveYearId,
                    cancellationToken);

            if (targetLeaveYear is null || targetLeaveYear.IsDeleted)
            {
                throw new NotFoundException(
                    "Leave Year",
                    request.LeaveYearId);
            }

            // 2. Find immediately previous leave year
            var leaveYears =
                await _leaveYearRepository.FindAsync(
                    x =>
                        x.CompanyId == targetLeaveYear.CompanyId &&
                        !x.IsDeleted &&
                        x.EndDate < targetLeaveYear.StartDate,
                    cancellationToken);

            var previousLeaveYear = leaveYears
                .OrderByDescending(x => x.EndDate)
                .FirstOrDefault();

            if (previousLeaveYear is null)
            {
                throw new ConflictException(
                    "Previous Leave Year could not be found.");
            }

            // 3. Get previous-year entitlements
            var previousEntitlements =
                await _entitlementReadRepository.FindAsync(
                    x =>
                        x.LeaveYearId == previousLeaveYear.Id &&
                        !x.IsDeleted,
                    cancellationToken);

            var newEntitlements =
                new List<EmployeeLeaveEntitlement>();

            // 4. Calculate carry forward
            foreach (var previousEntitlement in previousEntitlements)
            {
                // Idempotency:
                // Do not create another target-year entitlement
                // if one already exists.
                var targetExists =
                    await _entitlementReadRepository.AnyAsync(
                        x =>
                            x.EmployeeId == previousEntitlement.EmployeeId &&
                            x.LeaveYearId == targetLeaveYear.Id &&
                            x.LeaveTypeId == previousEntitlement.LeaveTypeId &&
                            !x.IsDeleted,
                        cancellationToken);

                if (targetExists)
                {
                    continue;
                }

                // Get policy rule
                var policyRule =
                    await _policyRuleRepository.GetByIdAsync(
                        previousEntitlement.LeavePolicyRuleId,
                        cancellationToken);

                if (policyRule is null || policyRule.IsDeleted)
                {
                    throw new NotFoundException(
                        "Leave Policy Rule",
                        previousEntitlement.LeavePolicyRuleId);
                }

                // Closing balance from previous year
                var closingBalance =
                    Math.Max(
                        0,
                        previousEntitlement.EntitledDays
                        - previousEntitlement.UsedDays);

                // Calculate carry forward
                decimal carryForwardDays = 0;

                if (policyRule.IsCarryForwardAllowed)
                {
                    carryForwardDays =
                        Math.Min(
                            closingBalance,
                            policyRule.MaximumCarryForwardDays);
                }

                // Create next-year entitlement
                var newEntitlement =
                    new EmployeeLeaveEntitlement
                    {
                        EmployeeId = previousEntitlement.EmployeeId,
                        LeaveYearId = targetLeaveYear.Id,
                        LeaveTypeId = previousEntitlement.LeaveTypeId,
                        LeavePolicyRuleId =
                            previousEntitlement.LeavePolicyRuleId,
                        EntitledDays =
                            policyRule.AnnualEntitlement,
                        CarryForwardDays =
                            carryForwardDays,
                        UsedDays = 0
                    };

                newEntitlements.Add(newEntitlement);
            }

            // 5. Insert all missing entitlements
            if (newEntitlements.Count > 0)
            {
                await _entitlementWriteRepository.AddRangeAsync(
                    newEntitlements,
                    cancellationToken);
            }

            // 6. Commit everything together
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            // Any failure means the complete operation is rolled back.
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
