using HRMS.Modules.Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeExperienceConfiguration
    : IEntityTypeConfiguration<EmployeeExperience>
{
    public void Configure(
        EntityTypeBuilder<EmployeeExperience> builder)
    {
        builder.ToTable("EmployeeExperiences");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.CompanyName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.JobTitle)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.EmploymentType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.StartDate)
               .IsRequired();

        builder.Property(x => x.EndDate);

        builder.Property(x => x.Location)
               .HasMaxLength(150);

        builder.Property(x => x.Responsibilities)
               .HasMaxLength(2000);

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
