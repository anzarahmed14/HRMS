using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.BusinessRules;

public class MaritalStatusBusinessRules
{
    private readonly IReadRepository<MaritalStatus, Guid> _repository;

    public MaritalStatusBusinessRules(
        IReadRepository<MaritalStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureMaritalStatusExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "MaritalStatus",
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
                $"Marital status code '{code}' already exists.");
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
                $"Marital status code '{code}' already exists.");
        }
    }
}
