using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.UpdateAttendanceDevice;

public sealed record UpdateAttendanceDeviceCommand(
    Guid Id,
    string Code,
    string Name,
    string? SerialNumber,
    string? IpAddress,
    string? Location,
    bool IsActive
) : IRequest;
