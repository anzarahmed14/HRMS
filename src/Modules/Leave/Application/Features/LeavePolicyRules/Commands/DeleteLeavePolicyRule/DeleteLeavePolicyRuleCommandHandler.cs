using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.DeleteLeavePolicyRule;

public class DeleteLeavePolicyRuleCommandHandler
    : IRequestHandler<DeleteLeavePolicyRuleCommand>
{
    private readonly IReadRepository<LeavePolicyRule, Guid> _readRepository;
    private readonly IWriteRepository<LeavePolicyRule, Guid> _writeRepository;
    private readonly LeavePolicyRuleBusinessRules _businessRules;

    public DeleteLeavePolicyRuleCommandHandler(
        IReadRepository<LeavePolicyRule, Guid> readRepository,
        IWriteRepository<LeavePolicyRule, Guid> writeRepository,
        LeavePolicyRuleBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLeavePolicyRuleCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}
