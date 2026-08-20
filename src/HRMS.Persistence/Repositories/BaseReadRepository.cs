using System.Linq.Expressions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.BuildingBlocks.Domain.Entities;
using HRMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Repositories;

public class BaseReadRepository<TEntity, TKey>
    : IReadRepository<TEntity, TKey>
    where TEntity : AuditableEntity<TKey>
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseReadRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = CreateQuery(includes);

        return await query.FirstOrDefaultAsync(
            e => EF.Property<TKey>(e, "Id")!.Equals(id),
            cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = CreateQuery(includes);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = CreateQuery(includes);

        return await query
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = CreateQuery(includes);

        return await query.FirstOrDefaultAsync(
            predicate,
            cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetPagedAsync(
        PagedRequest request,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = CreateQuery(includes);

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var totalRecords = await query.CountAsync(cancellationToken);

        if (orderBy is not null)
        {
            query = orderBy(query);
        }
        else if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.Descending
                ? query.OrderByDescending(
                    e => EF.Property<object>(e, request.SortBy!))
                : query.OrderBy(
                    e => EF.Property<object>(e, request.SortBy!));
        }

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

    private IQueryable<TEntity> CreateQuery(
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        return ApplyIncludes(query, includes);
    }

    private static IQueryable<TEntity> ApplyIncludes(
        IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[] includes)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }
}