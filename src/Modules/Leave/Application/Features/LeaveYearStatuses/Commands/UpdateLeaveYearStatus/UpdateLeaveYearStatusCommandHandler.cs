using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.UpdateLeaveYearStatus;

public class UpdateLeaveYearStatusCommandHandler
    : IRequestHandler<UpdateLeaveYearStatusCommand>
{
    private readonly IReadRepository<LeaveYearStatus, Guid>
        _readRepository;

    private readonly IWriteRepository<LeaveYearStatus, Guid>
        _writeRepository;

    private readonly LeaveYearStatusBusinessRules
        _businessRules;

    public UpdateLeaveYearStatusCommandHandler(
        IReadRepository<LeaveYearStatus, Guid> readRepository,
        IWriteRepository<LeaveYearStatus, Guid> writeRepository,
        LeaveYearStatusBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeaveYearStatusCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureLeaveYearStatusExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        entity.Code = code;
        entity.Name = request.Name.Trim();
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
