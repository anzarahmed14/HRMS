using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDevices.Queries.GetAttendanceDevices;

public sealed record GetAttendanceDevicesQuery(
    PagedRequest Request
) : IRequest<PagedResult<AttendanceDeviceDto>>;
