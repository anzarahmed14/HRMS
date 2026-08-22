namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.UpdateAttendanceRegularization;

public sealed record UpdateAttendanceRegularizationRequest(
    DateTimeOffset? RequestedCheckIn,
    DateTimeOffset? RequestedCheckOut,
    string Reason);
