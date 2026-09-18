using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.UpdateAttendanceDayStatus;

public sealed record UpdateAttendanceDayStatusCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive
) : IRequest;
