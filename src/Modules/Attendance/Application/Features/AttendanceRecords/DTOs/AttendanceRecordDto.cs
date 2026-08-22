namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.DTOs;

public sealed class AttendanceRecordDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid AttendanceShiftId { get; set; }

    public Guid AttendancePolicyId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public DateTimeOffset? CheckIn { get; set; }

    public DateTimeOffset? CheckOut { get; set; }

    public int WorkedMinutes { get; set; }

    public int LateMinutes { get; set; }

    public int EarlyLeaveMinutes { get; set; }

    public int OvertimeMinutes { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}
