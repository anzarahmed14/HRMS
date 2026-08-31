using HRMS.Modules.Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeEducationConfiguration
    : IEntityTypeConfiguration<EmployeeEducation>
{
    public void Configure(
        EntityTypeBuilder<EmployeeEducation> builder)
    {
        builder.ToTable("EmployeeEducations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.EducationLevel)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Qualification)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Specialization)
               .HasMaxLength(200);

        builder.Property(x => x.InstitutionName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.UniversityName)
               .HasMaxLength(200);

        builder.Property(x => x.StartDate);

        builder.Property(x => x.EndDate);

        builder.Property(x => x.Grade)
               .HasMaxLength(50);

        builder.Property(x => x.IsHighestQualification)
               .IsRequired();

        builder.Property(x => x.IsVerified)
               .IsRequired();

        builder.Property(x => x.VerifiedOn);

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
