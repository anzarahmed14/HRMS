using HRMS.Application.Features.Dependents.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Dependents.Queries.GetEmployeeDependents;

public sealed record GetEmployeeDependentsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeDependentDto>>;
