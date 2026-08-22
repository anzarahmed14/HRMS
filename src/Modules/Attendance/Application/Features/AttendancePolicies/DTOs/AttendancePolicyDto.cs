namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;

public class AttendancePolicyDto
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int GracePeriodMinutes { get; set; }

    public int MinimumWorkingMinutes { get; set; }

    public int FullDayMinutes { get; set; }

    public int HalfDayMinutes { get; set; }

    public bool IsOvertimeAllowed { get; set; }

    public int MinimumOvertimeMinutes { get; set; }

    public int MaximumOvertimeMinutes { get; set; }

    public bool OvertimeRequiresApproval { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}
