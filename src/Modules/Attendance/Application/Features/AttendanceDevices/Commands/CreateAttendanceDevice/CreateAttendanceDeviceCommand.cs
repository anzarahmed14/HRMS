using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.CreateAttendanceDevice;

public sealed record CreateAttendanceDeviceCommand(
    Guid AttendanceSourceId,
    string Code,
    string Name,
    string? SerialNumber,
    string? IpAddress,
    string? Location,
    bool IsActive
) : IRequest<Guid>;
