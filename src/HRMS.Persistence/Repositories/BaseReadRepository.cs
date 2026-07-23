using System.Linq.Expressions;
using HRMS.Domain.Interfaces;
using HRMS.Persistence.Context;
using HRMS.Shared.Entities;
using HRMS.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Repositories;

public class BaseReadRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>
    where TEntity : AuditableEntity<TKey>
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseReadRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query.FirstOrDefaultAsync(
            e => EF.Property<TKey>(e, "Id")!.Equals(id),
            cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
         CancellationToken cancellationToken = default,
         params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default,
    params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    private IQueryable<TEntity> ApplyIncludes(
    IQueryable<TEntity> query,
    params Expression<Func<TEntity, object>>[] includes)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }

   public async Task<PagedResult<TEntity>> GetPagedAsync(
    PagedRequest request,
    Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    CancellationToken cancellationToken = default,
    params Expression<Func<TEntity, object>>[] includes)
{
    IQueryable<TEntity> query = _dbSet.AsNoTracking();

    // Apply Includes
    query = ApplyIncludes(query, includes);

    // Apply Filter
    if (predicate is not null)
    {
        query = query.Where(predicate);
    }

    // Total Records before Paging
    var totalRecords = await query.CountAsync(cancellationToken);

    // Apply Sorting
    if (orderBy is not null)
    {
        query = orderBy(query);
    }
    else if (!string.IsNullOrWhiteSpace(request.SortBy))
    {
        query = request.Descending
            ? query.OrderByDescending(e => EF.Property<object>(e, request.SortBy!))
            : query.OrderBy(e => EF.Property<object>(e, request.SortBy!));
    }

    // Apply Paging
    query = query
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize);

    var items = await query.ToListAsync(cancellationToken);

    return new PagedResult<TEntity>
    {
        Items = items,
        TotalRecords = totalRecords,
        PageNumber = request.PageNumber,
        PageSize = request.PageSize
       
    };
}
}