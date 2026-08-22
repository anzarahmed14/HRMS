using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Queries.GetEmployeeShiftAssignments;

public sealed record GetEmployeeShiftAssignmentsQuery(
    PagedRequest Request
) : IRequest<PagedResult<EmployeeShiftAssignmentDto>>;
