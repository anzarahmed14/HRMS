using HRMS.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Identity.Infrastructure.Configurations;

public class RolePermissionConfiguration
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Permission)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.RoleId);

        builder.HasIndex(x => x.PermissionId);

        builder.HasIndex(x => new
        {
            x.RoleId,
            x.PermissionId
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}