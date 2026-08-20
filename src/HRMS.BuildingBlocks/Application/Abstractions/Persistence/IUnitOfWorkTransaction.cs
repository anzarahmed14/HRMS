namespace HRMS.BuildingBlocks.Application.Abstractions.Persistence;

public interface IUnitOfWorkTransaction
{
    Task BeginAsync(
        CancellationToken cancellationToken = default);

    Task CommitAsync(
        CancellationToken cancellationToken = default);

    Task RollbackAsync(
        CancellationToken cancellationToken = default);
}
