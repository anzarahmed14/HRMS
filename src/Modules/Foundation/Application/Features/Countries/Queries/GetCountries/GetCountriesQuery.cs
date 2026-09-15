using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Countries.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Queries.GetCountries;

public sealed record GetCountriesQuery(
    PagedRequest Request) : IRequest<PagedResult<CountryDto>>;
