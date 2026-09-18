using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.DeleteAttendanceDayStatus;

public sealed record DeleteAttendanceDayStatusCommand(
    Guid Id
) : IRequest;
