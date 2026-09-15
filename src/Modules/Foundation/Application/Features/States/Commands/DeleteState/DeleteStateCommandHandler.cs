using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.States.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.DeleteState;

public sealed class DeleteStateCommandHandler
    : IRequestHandler<DeleteStateCommand>
{
    private readonly IReadRepository<State, Guid> _readRepository;
    private readonly IWriteRepository<State, Guid> _writeRepository;
    private readonly StateBusinessRules _businessRules;

    public DeleteStateCommandHandler(
        IReadRepository<State, Guid> readRepository,
        IWriteRepository<State, Guid> writeRepository,
        StateBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteStateCommand request,
        CancellationToken cancellationToken)
    {
        var state = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (state is null || state.IsDeleted)
        {
            await _businessRules.EnsureStateExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            state,
            cancellationToken);
    }
}
