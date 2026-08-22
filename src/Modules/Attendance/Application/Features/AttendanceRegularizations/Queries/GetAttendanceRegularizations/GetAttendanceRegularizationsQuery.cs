using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizations;

public sealed record GetAttendanceRegularizationsQuery(
    Guid? EmployeeId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    Guid? StatusId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<AttendanceRegularizationDto>>;
