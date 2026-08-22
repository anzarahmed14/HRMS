using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Attendance.Domain.Entities;

public class AttendanceRawLog : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid AttendanceDeviceId { get; set; }

    public DateTimeOffset PunchDateTime { get; set; }

    public string PunchType { get; set; } = string.Empty;

    public string? ExternalRecordId { get; set; }

    public string? RawData { get; set; }

    public DateTimeOffset ImportedOn { get; set; }
}
