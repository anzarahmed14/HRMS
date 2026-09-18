namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.DTOs;

public sealed class AttendanceRegularizationStatusDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
