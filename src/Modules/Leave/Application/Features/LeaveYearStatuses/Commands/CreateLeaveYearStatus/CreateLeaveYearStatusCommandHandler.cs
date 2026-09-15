using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.CreateLeaveYearStatus;

public class CreateLeaveYearStatusCommandHandler
    : IRequestHandler<CreateLeaveYearStatusCommand, Guid>
{
    private readonly IWriteRepository<LeaveYearStatus, Guid>
        _writeRepository;

    private readonly LeaveYearStatusBusinessRules
        _businessRules;

    public CreateLeaveYearStatusCommandHandler(
        IWriteRepository<LeaveYearStatus, Guid> writeRepository,
        LeaveYearStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeaveYearStatusCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var entity = new LeaveYearStatus
        {
            Code = code,
            Name = request.Name.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
