namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.DTOs;

public sealed class AttendanceDeviceDto
{
    public Guid Id { get; set; }

    public Guid AttendanceSourceId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? SerialNumber { get; set; }

    public string? IpAddress { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; }
}
