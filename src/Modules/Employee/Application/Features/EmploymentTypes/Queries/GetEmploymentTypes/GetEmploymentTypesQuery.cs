using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Application.Features.EmploymentTypes.DTOs;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Queries.GetEmploymentTypes;

public sealed record GetEmploymentTypesQuery(
    PagedRequest Request
) : IRequest<PagedResult<EmploymentTypeDto>>;
