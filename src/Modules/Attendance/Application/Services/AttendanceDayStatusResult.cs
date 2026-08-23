namespace HRMS.Modules.Attendance.Application.Services;

public sealed record AttendanceDayStatusResult(
    Guid StatusId,
    string Code,
    string? Remarks);