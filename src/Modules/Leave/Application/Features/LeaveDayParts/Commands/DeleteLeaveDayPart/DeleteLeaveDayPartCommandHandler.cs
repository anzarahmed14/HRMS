using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.DeleteLeaveDayPart;

public class DeleteLeaveDayPartCommandHandler
    : IRequestHandler<DeleteLeaveDayPartCommand>
{
    private readonly IReadRepository<LeaveDayPart, Guid> _readRepository;
    private readonly IWriteRepository<LeaveDayPart, Guid> _writeRepository;
    private readonly LeaveDayPartBusinessRules _businessRules;

    public DeleteLeaveDayPartCommandHandler(
        IReadRepository<LeaveDayPart, Guid> readRepository,
        IWriteRepository<LeaveDayPart, Guid> writeRepository,
        LeaveDayPartBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteLeaveDayPartCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureLeaveDayPartExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}
