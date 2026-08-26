using HRMS.Application.Features.EmploymentStatuses.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Queries.GetEmploymentStatuses;

public sealed record GetEmploymentStatusesQuery(
    PagedRequest Request
) : IRequest<PagedResult<EmploymentStatusDto>>;
