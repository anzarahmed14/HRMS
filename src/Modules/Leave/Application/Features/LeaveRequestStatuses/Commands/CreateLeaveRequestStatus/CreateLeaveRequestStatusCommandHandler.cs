using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.CreateLeaveRequestStatus;

public class CreateLeaveRequestStatusCommandHandler
    : IRequestHandler<CreateLeaveRequestStatusCommand, Guid>
{
    private readonly IWriteRepository<LeaveRequestStatus, Guid> _writeRepository;
    private readonly LeaveRequestStatusBusinessRules _businessRules;

    public CreateLeaveRequestStatusCommandHandler(
        IWriteRepository<LeaveRequestStatus, Guid> writeRepository,
        LeaveRequestStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeaveRequestStatusCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var entity = new LeaveRequestStatus
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
