using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.BusinessRules;

public class LeaveDayPartBusinessRules
{
    private readonly IReadRepository<LeaveDayPart, Guid> _repository;

    public LeaveDayPartBusinessRules(
        IReadRepository<LeaveDayPart, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureLeaveDayPartExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "LeaveDayPart",
                id);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x =>
                x.Code == code &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Leave day part code '{code}' already exists.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x =>
                x.Id != id &&
                x.Code == code &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Leave day part code '{code}' already exists.");
        }
    }
}
