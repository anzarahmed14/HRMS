using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Attendance.Domain.Entities;

public class EmployeeShiftAssignment : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid AttendanceShiftId { get; set; }

    public Guid AttendancePolicyId { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; }
}