using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.UpdateAttendanceRegularization;

public sealed record UpdateAttendanceRegularizationCommand(
    Guid Id,
    DateTimeOffset? RequestedCheckIn,
    DateTimeOffset? RequestedCheckOut,
    string Reason
) : IRequest;
