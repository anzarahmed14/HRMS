using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Modules.Foundation.Application.Features.States.BusinessRules;

public sealed class StateBusinessRules
{
    private readonly IReadRepository<State, Guid> _repository;

    public StateBusinessRules(
        IReadRepository<State, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureStateExistsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var state = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (state is null || state.IsDeleted)
        {
            throw new NotFoundException(
                "State",
                id);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        Guid countryId,
        string code,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.CountryId == countryId &&
                 x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "State code already exists for this country.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        Guid countryId,
        string code,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.CountryId == countryId &&
                 x.Id != id &&
                 x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "State code already exists for this country.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        Guid countryId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.CountryId == countryId &&
                 x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "State name already exists for this country.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        Guid countryId,
        string name,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.CountryId == countryId &&
                 x.Id != id &&
                 x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "State name already exists for this country.");
        }
    }
}
