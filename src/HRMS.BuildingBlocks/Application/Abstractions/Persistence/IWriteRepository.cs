using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.BuildingBlocks.Application.Abstractions.Persistence;

public interface IWriteRepository<TEntity, TKey>
    where TEntity :   AuditableEntity<TKey>
{
    Task<TEntity> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);
}