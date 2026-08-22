namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.DTOs;

public sealed class EmployeeShiftAssignmentDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid AttendanceShiftId { get; set; }

    public Guid AttendancePolicyId { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; }
}
