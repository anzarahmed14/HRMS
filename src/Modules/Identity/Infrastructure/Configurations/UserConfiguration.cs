
using HRMS.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Identity.Infrastructure.Configurations;


public class UserConfiguration : IEntityTypeConfiguration< HRMS.Modules.Identity.Domain.Entities.User>
{
    public void Configure(EntityTypeBuilder< HRMS.Modules.Identity.Domain.Entities.User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .IsRequired();

        //builder.HasOne(x => x.Employee)
        //    .WithOne()
        //    .HasForeignKey<User>(x => x.EmployeeId)
        //    .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId)
            .IsUnique();

        builder.HasIndex(x => x.UserName)
            .IsUnique();
    }
}