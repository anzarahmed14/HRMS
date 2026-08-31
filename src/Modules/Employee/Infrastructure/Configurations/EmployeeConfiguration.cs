using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<HRMS.Modules.Employee.Domain.Entities.Employee>
{
    public void Configure(
        EntityTypeBuilder<HRMS.Modules.Employee.Domain.Entities.Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeCode)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(x => x.DateOfBirth)
               .IsRequired();

        builder.Property(x => x.DateOfJoining)
               .IsRequired();

        builder.Property(x => x.DepartmentId)
               .IsRequired();

        builder.Property(x => x.GenderId)
               .IsRequired();

        builder.Property(x => x.MaritalStatusId)
               .IsRequired();

        // Department ? Employee
        builder.HasOne<HRMS.Modules.Department.Domain.Entities.Department>()
               .WithMany()
               .HasForeignKey(x => x.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        // Gender ? Employee
        builder.HasOne<HRMS.Modules.Foundation.Domain.Entities.Gender>()
               .WithMany()
               .HasForeignKey(x => x.GenderId)
               .OnDelete(DeleteBehavior.Restrict);

        // MaritalStatus ? Employee
        builder.HasOne<HRMS.Modules.Foundation.Domain.Entities.MaritalStatus>()
               .WithMany()
               .HasForeignKey(x => x.MaritalStatusId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeCode)
               .IsUnique();

        builder.HasIndex(x => x.Email)
               .IsUnique();

        builder.HasIndex(x => x.DepartmentId);

        builder.HasIndex(x => x.GenderId);

        builder.HasIndex(x => x.MaritalStatusId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
