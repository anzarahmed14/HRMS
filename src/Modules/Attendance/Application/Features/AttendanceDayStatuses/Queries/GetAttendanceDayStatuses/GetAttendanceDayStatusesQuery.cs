using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Queries.GetAttendanceDayStatuses;

public sealed record GetAttendanceDayStatusesQuery(
    PagedRequest Request
) : IRequest<PagedResult<AttendanceDayStatusDto>>;
