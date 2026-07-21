using HRMS.Domain.Entities;
using HRMS.Shared.Entities;
using HRMS.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IUserContext _userContext;
    public ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IUserContext userContext)
    : base(options)
{
    _userContext = userContext;
}

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<AuditableEntity<Guid>>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = _userContext.UserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTimeOffset.UtcNow;
                    entry.Entity.ModifiedBy = _userContext.UserId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;

                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedOn = DateTimeOffset.UtcNow;
                    entry.Entity.DeletedBy = _userContext.UserId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}