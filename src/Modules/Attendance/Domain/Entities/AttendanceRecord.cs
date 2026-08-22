using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Attendance.Domain.Entities;

public class AttendanceRecord : AuditableEntity<Guid>
{
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
