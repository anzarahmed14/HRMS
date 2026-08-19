using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.UpdateLeavePolicyRule;

public class UpdateLeavePolicyRuleCommandHandler
    : IRequestHandler<UpdateLeavePolicyRuleCommand>
{
    private readonly IReadRepository<LeavePolicyRule, Guid> _readRepository;
    private readonly IWriteRepository<LeavePolicyRule, Guid> _writeRepository;
    private readonly LeavePolicyRuleBusinessRules _businessRules;

    public UpdateLeavePolicyRuleCommandHandler(
        IReadRepository<LeavePolicyRule, Guid> readRepository,
        IWriteRepository<LeavePolicyRule, Guid> writeRepository,
        LeavePolicyRuleBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeavePolicyRuleCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsurePolicyRuleExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _businessRules.EnsurePolicyExistsAsync(
            request.LeavePolicyId,
            cancellationToken);

        await _businessRules.EnsureLeaveTypeExistsAsync(
            request.LeaveTypeId,
            cancellationToken);

        await _businessRules.EnsureUniquePolicyLeaveTypeAsync(
            request.LeavePolicyId,
            request.LeaveTypeId,
            request.Id,
            cancellationToken);

        _businessRules.EnsureAnnualEntitlementIsValid(
            request.AnnualEntitlement);

        entity.LeavePolicyId = request.LeavePolicyId;
        entity.LeaveTypeId = request.LeaveTypeId;
        entity.AnnualEntitlement = request.AnnualEntitlement;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
