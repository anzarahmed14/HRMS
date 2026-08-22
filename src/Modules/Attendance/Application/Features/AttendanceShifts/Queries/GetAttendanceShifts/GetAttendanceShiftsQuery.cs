using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Queries.GetAttendanceShifts;

public sealed record GetAttendanceShiftsQuery(
    PagedRequest Request
) : IRequest<PagedResult<AttendanceShiftDto>>;
