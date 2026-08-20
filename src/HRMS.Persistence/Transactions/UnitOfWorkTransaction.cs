using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRMS.Persistence.Transactions;

public sealed class UnitOfWorkTransaction : IUnitOfWorkTransaction
{
    private readonly ApplicationDbContext _context;

    private IDbContextTransaction? _transaction;

    public UnitOfWorkTransaction(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task BeginAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            return;
        }

        _transaction = await _context.Database.BeginTransactionAsync(
            cancellationToken);
    }

    public async Task CommitAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        await _context.SaveChangesAsync(cancellationToken);

        await _transaction.CommitAsync(cancellationToken);

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync(cancellationToken);

        await _transaction.DisposeAsync();

        _transaction = null;
    }
}
