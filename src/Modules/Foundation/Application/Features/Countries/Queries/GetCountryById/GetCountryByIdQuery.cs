using HRMS.Modules.Foundation.Application.Features.Countries.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Queries.GetCountryById;

public sealed record GetCountryByIdQuery(
    Guid Id) : IRequest<CountryDto>;
