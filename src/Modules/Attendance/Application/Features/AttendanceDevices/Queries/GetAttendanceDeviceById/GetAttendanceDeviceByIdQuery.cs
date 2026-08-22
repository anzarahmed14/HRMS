using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Queries.GetAttendanceDeviceById;

public sealed record GetAttendanceDeviceByIdQuery(
    Guid Id
) : IRequest<AttendanceDeviceDto>;
