using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeSkillConfiguration
    : IEntityTypeConfiguration<EmployeeSkill>
{
    public void Configure(
        EntityTypeBuilder<EmployeeSkill> builder)
    {
        builder.ToTable("EmployeeSkills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.SkillId)
               .IsRequired();

        builder.Property(x => x.ProficiencyLevel)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.YearsOfExperience)
               .HasPrecision(5, 2)
               .IsRequired();

        builder.Property(x => x.IsPrimary)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Skill>()
               .WithMany()
               .HasForeignKey(x => x.SkillId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.SkillId);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.SkillId
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        // One primary skill per employee
        builder.HasIndex(x => x.EmployeeId)
               .IsUnique()
               .HasFilter("[IsPrimary] = 1 AND [IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
