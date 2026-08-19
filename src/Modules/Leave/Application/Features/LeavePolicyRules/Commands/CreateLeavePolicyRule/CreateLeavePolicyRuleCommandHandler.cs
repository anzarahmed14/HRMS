using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.CreateLeavePolicyRule;

public class CreateLeavePolicyRuleCommandHandler
    : IRequestHandler<CreateLeavePolicyRuleCommand, Guid>
{
    private readonly IWriteRepository<LeavePolicyRule, Guid>
        _writeRepository;

    private readonly LeavePolicyRuleBusinessRules
        _businessRules;

    public CreateLeavePolicyRuleCommandHandler(
        IWriteRepository<LeavePolicyRule, Guid> writeRepository,
        LeavePolicyRuleBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeavePolicyRuleCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsurePolicyExistsAsync(
            request.LeavePolicyId,
            cancellationToken);

        await _businessRules.EnsureLeaveTypeExistsAsync(
            request.LeaveTypeId,
            cancellationToken);

        await _businessRules.EnsureUniquePolicyLeaveTypeAsync(
            request.LeavePolicyId,
            request.LeaveTypeId,
            cancellationToken);

        _businessRules.EnsureAnnualEntitlementIsValid(
            request.AnnualEntitlement);

        var entity = new LeavePolicyRule
        {
            LeavePolicyId = request.LeavePolicyId,
            LeaveTypeId = request.LeaveTypeId,
            AnnualEntitlement = request.AnnualEntitlement
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}