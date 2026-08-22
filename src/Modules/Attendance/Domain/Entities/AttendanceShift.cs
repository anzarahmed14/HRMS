using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Attendance.Domain.Entities;

public class AttendanceShift : AuditableEntity<Guid>
{
    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int BreakMinutes { get; set; }

    public bool IsOvernight { get; set; }

    public bool IsActive { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}
