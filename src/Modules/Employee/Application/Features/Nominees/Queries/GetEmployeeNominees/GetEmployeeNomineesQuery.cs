using HRMS.Application.Features.Nominees.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Nominees.Queries.GetEmployeeNominees;

public sealed record GetEmployeeNomineesQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeNomineeDto>>;
