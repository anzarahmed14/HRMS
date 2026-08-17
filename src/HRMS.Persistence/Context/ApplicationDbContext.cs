using HRMS.BuildingBlocks.Domain.Entities;
using HRMS.BuildingBlocks.Application.Abstractions;

using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Department.Domain.Entities;


using Microsoft.EntityFrameworkCore;
using HRMS.Modules.Identity.Domain.Entities;
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

     public DbSet<User> Users => Set<User>();

     public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();    

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

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