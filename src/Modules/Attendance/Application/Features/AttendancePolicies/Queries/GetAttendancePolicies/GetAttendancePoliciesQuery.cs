using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Queries.GetAttendancePolicies;

public sealed record GetAttendancePoliciesQuery(
    PagedRequest Request
) : IRequest<PagedResult<AttendancePolicyDto>>;
