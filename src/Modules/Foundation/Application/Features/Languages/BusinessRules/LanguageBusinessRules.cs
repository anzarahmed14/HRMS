using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Modules.Foundation.Application.Features.Languages.BusinessRules;

public class LanguageBusinessRules
{
    private readonly IReadRepository<Language, Guid> _repository;

    public LanguageBusinessRules(
        IReadRepository<Language, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureLanguageExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Language",
                id);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x => x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Language code '{code}' already exists.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x => x.Id != id &&
                 x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Language code '{code}' already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x => x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Language name '{name}' already exists.");
        }
    }

    public async Task EnsureNameUniqueAsync(
        string name,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x => x.Id != id &&
                 x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Language name '{name}' already exists.");
        }
    }
}
