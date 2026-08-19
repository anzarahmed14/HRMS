using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.DeleteLeavePolicy;

public class DeleteLeavePolicyCommandHandler
    : IRequestHandler<DeleteLeavePolicyCommand>
{
    private readonly IReadRepository<LeavePolicy, Guid> _readRepository;
    private readonly IWriteRepository<LeavePolicy, Guid> _writeRepository;
    private readonly LeavePolicyBusinessRules _businessRules;

    public DeleteLeavePolicyCommandHandler(
        IReadRepository<LeavePolicy, Guid> readRepository,
        IWriteRepository<LeavePolicy, Guid> writeRepository,
        LeavePolicyBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLeavePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            await _businessRules.EnsureLeavePolicyExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}
