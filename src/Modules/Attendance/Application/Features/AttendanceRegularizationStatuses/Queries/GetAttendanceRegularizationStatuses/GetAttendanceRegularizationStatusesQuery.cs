using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Queries.GetAttendanceRegularizationStatuses;

public sealed record GetAttendanceRegularizationStatusesQuery(
    PagedRequest Request
) : IRequest<PagedResult<AttendanceRegularizationStatusDto>>;
