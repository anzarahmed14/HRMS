using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Modules.Foundation.Application.Features.Countries.BusinessRules;

public sealed class CountryBusinessRules
{
    private readonly IReadRepository<Country, Guid> _repository;

    public CountryBusinessRules(
        IReadRepository<Country, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureCountryExistsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var country = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (country is null || country.IsDeleted)
        {
            throw new NotFoundException(
                "Country",
                id);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Country code already exists.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.Id != id &&
                 x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Country code already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Country name already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.Id != id &&
                 x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Country name already exists.");
        }
    }
}
