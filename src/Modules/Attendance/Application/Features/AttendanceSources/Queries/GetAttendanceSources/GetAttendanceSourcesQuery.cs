using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Queries.GetAttendanceSources;

public sealed record GetAttendanceSourcesQuery(
    PagedRequest Request
) : IRequest<PagedResult<AttendanceSourceDto>>;
