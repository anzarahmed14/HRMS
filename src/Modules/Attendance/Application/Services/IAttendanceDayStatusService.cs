namespace HRMS.Modules.Attendance.Application.Services;

public interface IAttendanceDayStatusService
{
    Task<AttendanceDayStatusResult> DetermineAsync(
        Guid employeeId,
        DateOnly date,
        CancellationToken cancellationToken = default);
}
