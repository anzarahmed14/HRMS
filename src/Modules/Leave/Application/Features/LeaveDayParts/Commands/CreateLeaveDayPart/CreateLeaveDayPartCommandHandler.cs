using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.CreateLeaveDayPart;

public class CreateLeaveDayPartCommandHandler
    : IRequestHandler<CreateLeaveDayPartCommand, Guid>
{
    private readonly IWriteRepository<LeaveDayPart, Guid> _writeRepository;
    private readonly LeaveDayPartBusinessRules _businessRules;

    public CreateLeaveDayPartCommandHandler(
        IWriteRepository<LeaveDayPart, Guid> writeRepository,
        LeaveDayPartBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeaveDayPartCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var entity = new LeaveDayPart
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            DaysValue = request.DaysValue,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
