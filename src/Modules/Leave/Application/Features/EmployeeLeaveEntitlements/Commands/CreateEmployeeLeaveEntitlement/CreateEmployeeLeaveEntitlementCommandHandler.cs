using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.CreateEmployeeLeaveEntitlement;

public class CreateEmployeeLeaveEntitlementCommandHandler
    : IRequestHandler<CreateEmployeeLeaveEntitlementCommand, Guid>
{
    private readonly IWriteRepository<EmployeeLeaveEntitlement, Guid>
        _writeRepository;

    private readonly EmployeeLeaveEntitlementBusinessRules
        _businessRules;

    public CreateEmployeeLeaveEntitlementCommandHandler(
        IWriteRepository<EmployeeLeaveEntitlement, Guid> writeRepository,
        EmployeeLeaveEntitlementBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeLeaveEntitlementCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureLeaveYearExistsAsync(
            request.LeaveYearId,
            cancellationToken);

        await _businessRules.EnsureLeaveTypeExistsAsync(
            request.LeaveTypeId,
            cancellationToken);

        await _businessRules.EnsurePolicyRuleExistsAsync(
            request.LeavePolicyRuleId,
            cancellationToken);

        await _businessRules.EnsureUniqueEmployeeLeaveEntitlementAsync(
            request.EmployeeId,
            request.LeaveYearId,
            request.LeaveTypeId,
            cancellationToken);

        await _businessRules.EnsurePolicyRuleMatchesLeaveTypeAsync(
    request.LeavePolicyRuleId,
    request.LeaveTypeId,
    cancellationToken);

        _businessRules.EnsureEntitledDaysAreValid(
            request.EntitledDays);

        var entity = new EmployeeLeaveEntitlement
        {
            EmployeeId = request.EmployeeId,
            LeaveYearId = request.LeaveYearId,
            LeaveTypeId = request.LeaveTypeId,
            LeavePolicyRuleId = request.LeavePolicyRuleId,
            EntitledDays = request.EntitledDays
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}