using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Attendance.Domain.Entities;

public class AttendanceDevice : AuditableEntity<Guid>
{
    public Guid AttendanceSourceId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? SerialNumber { get; set; }

    public string? IpAddress { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; }
}
