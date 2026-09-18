using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.DeleteAttendanceDayStatus;

public sealed class DeleteAttendanceDayStatusCommandHandler
    : IRequestHandler<DeleteAttendanceDayStatusCommand>
{
    private readonly IWriteRepository<AttendanceDayStatus, Guid>
        _writeRepository;

    private readonly AttendanceDayStatusBusinessRules
        _businessRules;

    public DeleteAttendanceDayStatusCommandHandler(
        IWriteRepository<AttendanceDayStatus, Guid> writeRepository,
        AttendanceDayStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteAttendanceDayStatusCommand request,
        CancellationToken cancellationToken)
    {
        var dayStatus = await _businessRules.EnsureExistsAsync(
            request.Id,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            dayStatus,
            cancellationToken);
    }
}
