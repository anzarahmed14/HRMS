using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Countries.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Queries.GetCountries;

public sealed class GetCountriesQueryHandler
    : IRequestHandler<GetCountriesQuery, PagedResult<CountryDto>>
{
    private readonly IReadRepository<Country, Guid> _repository;

    public GetCountriesQueryHandler(
        IReadRepository<Country, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<CountryDto>> Handle(
        GetCountriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<CountryDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new CountryDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
