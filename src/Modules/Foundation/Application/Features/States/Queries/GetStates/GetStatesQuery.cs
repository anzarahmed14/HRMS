using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.States.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Queries.GetStates;

public sealed record GetStatesQuery(
    Guid CountryId,
    PagedRequest Request) : IRequest<PagedResult<StateDto>>;
