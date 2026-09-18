using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.CreateAttendanceDayStatus;

public sealed record CreateAttendanceDayStatusCommand(
    string Code,
    string Name,
    string? Description,
    bool IsActive
) : IRequest<Guid>;
