using HRMS.Modules.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Identity.Infrastructure.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    private static readonly Guid IdentityId =
        Guid.Parse("50000000-0000-0000-0000-000000000001");

    private static readonly Guid EmployeeId =
        Guid.Parse("50000000-0000-0000-0000-000000000002");

    private static readonly Guid FoundationId =
        Guid.Parse("50000000-0000-0000-0000-000000000003");

    private static readonly Guid CompaniesId =
        Guid.Parse("50000000-0000-0000-0000-000000000004");

    private static readonly Guid DepartmentId =
        Guid.Parse("50000000-0000-0000-0000-000000000005");

    private static readonly Guid AttendanceId =
        Guid.Parse("50000000-0000-0000-0000-000000000006");

    private static readonly Guid LeaveId =
        Guid.Parse("50000000-0000-0000-0000-000000000007");

    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Modules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(
            new Module
            {
                Id = IdentityId,
                Code = "IDENTITY",
                Name = "Identity",
                IsActive = true
            },
            new Module
            {
                Id = EmployeeId,
                Code = "EMPLOYEE",
                Name = "Employee",
                IsActive = true
            },
            new Module
            {
                Id = FoundationId,
                Code = "FOUNDATION",
                Name = "Foundation",
                IsActive = true
            },
            new Module
            {
                Id = CompaniesId,
                Code = "COMPANIES",
                Name = "Companies",
                IsActive = true
            },
            new Module
            {
                Id = DepartmentId,
                Code = "DEPARTMENT",
                Name = "Department",
                IsActive = true
            },
            new Module
            {
                Id = AttendanceId,
                Code = "ATTENDANCE",
                Name = "Attendance",
                IsActive = true
            },
            new Module
            {
                Id = LeaveId,
                Code = "LEAVE",
                Name = "Leave",
                IsActive = true
            });
    }
}
