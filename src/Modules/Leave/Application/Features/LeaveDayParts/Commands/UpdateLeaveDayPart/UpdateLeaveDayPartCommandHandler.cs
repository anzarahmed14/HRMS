using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.UpdateLeaveDayPart;

public class UpdateLeaveDayPartCommandHandler
    : IRequestHandler<UpdateLeaveDayPartCommand>
{
    private readonly IReadRepository<LeaveDayPart, Guid> _readRepository;
    private readonly IWriteRepository<LeaveDayPart, Guid> _writeRepository;
    private readonly LeaveDayPartBusinessRules _businessRules;

    public UpdateLeaveDayPartCommandHandler(
        IReadRepository<LeaveDayPart, Guid> readRepository,
        IWriteRepository<LeaveDayPart, Guid> writeRepository,
        LeaveDayPartBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeaveDayPartCommand request,
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

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        entity.Code = code;
        entity.Name = name;
        entity.DaysValue = request.DaysValue;
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
