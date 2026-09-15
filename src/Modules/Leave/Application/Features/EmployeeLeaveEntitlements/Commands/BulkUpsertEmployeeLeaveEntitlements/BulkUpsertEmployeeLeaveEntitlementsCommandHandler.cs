using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.BulkUpsertEmployeeLeaveEntitlements;

public class BulkUpsertEmployeeLeaveEntitlementsCommandHandler
    : IRequestHandler<
        BulkUpsertEmployeeLeaveEntitlementsCommand,
        BulkUpsertEmployeeLeaveEntitlementsResult>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _entitlementRepository;
    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid> _entitlementWriteRepository;
    private readonly EmployeeLeaveEntitlementBusinessRules _businessRules;
    private readonly IUnitOfWorkTransaction _transaction;

    public BulkUpsertEmployeeLeaveEntitlementsCommandHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> entitlementRepository,
        IWriteRepository<EmployeeLeaveEntitlement, Guid> entitlementWriteRepository,
        EmployeeLeaveEntitlementBusinessRules businessRules,
        IUnitOfWorkTransaction transaction)
    {
        _entitlementRepository = entitlementRepository;
        _entitlementWriteRepository = entitlementWriteRepository;
        _businessRules = businessRules;
        _transaction = transaction;
    }

    public async Task<BulkUpsertEmployeeLeaveEntitlementsResult> Handle(
        BulkUpsertEmployeeLeaveEntitlementsCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureLeaveYearExistsAsync(
            request.LeaveYearId,
            cancellationToken);

        foreach (var item in request.Entitlements)
        {
            await _businessRules.EnsureLeaveTypeExistsAsync(
                item.LeaveTypeId,
                cancellationToken);

            await _businessRules.EnsurePolicyRuleExistsAsync(
                item.LeavePolicyRuleId,
                cancellationToken);

            await _businessRules.EnsurePolicyRuleMatchesLeaveTypeAsync(
                item.LeavePolicyRuleId,
                item.LeaveTypeId,
                cancellationToken);

            _businessRules.EnsureEntitledDaysAreValid(item.EntitledDays);
        }

        var existingEntitlements =
            await _entitlementRepository.FindAsync(
                x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.LeaveYearId == request.LeaveYearId &&
                    !x.IsDeleted,
                cancellationToken);

        var existingByLeaveType = existingEntitlements
            .ToDictionary(x => x.LeaveTypeId);

        var createdCount = 0;
        var updatedCount = 0;

        await _transaction.BeginAsync(cancellationToken);

        try
        {
            foreach (var item in request.Entitlements)
            {
                if (existingByLeaveType.TryGetValue(
                        item.LeaveTypeId,
                        out var existing))
                {
                    existing.LeavePolicyRuleId = item.LeavePolicyRuleId;
                    existing.EntitledDays = item.EntitledDays;

                    await _entitlementWriteRepository.UpdateAsync(
                        existing,
                        cancellationToken);

                    updatedCount++;
                }
                else
                {
                    var entitlement = new EmployeeLeaveEntitlement
                    {
                        EmployeeId = request.EmployeeId,
                        LeaveYearId = request.LeaveYearId,
                        LeaveTypeId = item.LeaveTypeId,
                        LeavePolicyRuleId = item.LeavePolicyRuleId,
                        EntitledDays = item.EntitledDays,
                        CarryForwardDays = 0,
                        UsedDays = 0
                    };

                    await _entitlementWriteRepository.AddAsync(
                        entitlement,
                        cancellationToken);

                    createdCount++;
                }
            }

            await _transaction.CommitAsync(cancellationToken);

            return new BulkUpsertEmployeeLeaveEntitlementsResult
            {
                CreatedCount = createdCount,
                UpdatedCount = updatedCount
            };
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

