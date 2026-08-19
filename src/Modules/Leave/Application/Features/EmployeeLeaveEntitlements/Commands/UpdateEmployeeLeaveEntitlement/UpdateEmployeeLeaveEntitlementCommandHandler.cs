using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.UpdateEmployeeLeaveEntitlement;

public class UpdateEmployeeLeaveEntitlementCommandHandler
    : IRequestHandler<UpdateEmployeeLeaveEntitlementCommand>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid> _writeRepository;
    private readonly EmployeeLeaveEntitlementBusinessRules _businessRules;

    public UpdateEmployeeLeaveEntitlementCommandHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> readRepository,
        IWriteRepository<EmployeeLeaveEntitlement, Guid> writeRepository,
        EmployeeLeaveEntitlementBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateEmployeeLeaveEntitlementCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsureEntitlementExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _businessRules.EnsureLeaveYearExistsAsync(
            request.LeaveYearId,
            cancellationToken);

        await _businessRules.EnsureLeaveTypeExistsAsync(
            request.LeaveTypeId,
            cancellationToken);

        await _businessRules.EnsurePolicyRuleExistsAsync(
            request.LeavePolicyRuleId,
            cancellationToken);

        await _businessRules.EnsurePolicyRuleMatchesLeaveTypeAsync(
            request.LeavePolicyRuleId,
            request.LeaveTypeId,
            cancellationToken);

        await _businessRules.EnsureUniqueEmployeeLeaveEntitlementAsync(
            request.EmployeeId,
            request.LeaveYearId,
            request.LeaveTypeId,
            request.Id,
            cancellationToken);

        _businessRules.EnsureEntitledDaysAreValid(
            request.EntitledDays);

        entity.EmployeeId = request.EmployeeId;
        entity.LeaveYearId = request.LeaveYearId;
        entity.LeaveTypeId = request.LeaveTypeId;
        entity.LeavePolicyRuleId = request.LeavePolicyRuleId;
        entity.EntitledDays = request.EntitledDays;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
