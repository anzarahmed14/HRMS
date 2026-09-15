using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.UpdateLeaveRequestStatus;

public class UpdateLeaveRequestStatusCommandHandler
    : IRequestHandler<UpdateLeaveRequestStatusCommand>
{
    private readonly IReadRepository<LeaveRequestStatus, Guid> _readRepository;
    private readonly IWriteRepository<LeaveRequestStatus, Guid> _writeRepository;
    private readonly LeaveRequestStatusBusinessRules _businessRules;

    public UpdateLeaveRequestStatusCommandHandler(
        IReadRepository<LeaveRequestStatus, Guid> readRepository,
        IWriteRepository<LeaveRequestStatus, Guid> writeRepository,
        LeaveRequestStatusBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeaveRequestStatusCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureLeaveRequestStatusExistsAsync(
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
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
