namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.DTOs;

public sealed class AttendanceSourceDto
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string SourceType { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
