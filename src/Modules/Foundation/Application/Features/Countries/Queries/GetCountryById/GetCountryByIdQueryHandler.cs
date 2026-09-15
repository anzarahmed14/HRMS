using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.Countries.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Queries.GetCountryById;

public sealed class GetCountryByIdQueryHandler
    : IRequestHandler<GetCountryByIdQuery, CountryDto>
{
    private readonly IReadRepository<Country, Guid> _repository;

    public GetCountryByIdQueryHandler(
        IReadRepository<Country, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<CountryDto> Handle(
        GetCountryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var country = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (country is null || country.IsDeleted)
        {
            throw new NotFoundException(
                "Country",
                request.Id);
        }

        return new CountryDto
        {
            Id = country.Id,
            Code = country.Code,
            Name = country.Name,
            IsActive = country.IsActive
        };
    }
}
