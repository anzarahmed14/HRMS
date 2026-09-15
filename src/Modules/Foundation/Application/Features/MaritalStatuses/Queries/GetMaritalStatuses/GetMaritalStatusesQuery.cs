using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Queries.GetMaritalStatuses;

public record GetMaritalStatusesQuery(
    PagedRequest Request) : IRequest<PagedResult<MaritalStatusDto>>;
