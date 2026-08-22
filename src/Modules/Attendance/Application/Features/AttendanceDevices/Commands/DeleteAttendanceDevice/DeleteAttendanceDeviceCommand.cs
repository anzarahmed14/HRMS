using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Commands.DeleteAttendanceDevice;

public sealed record DeleteAttendanceDeviceCommand(
    Guid Id
) : IRequest;
