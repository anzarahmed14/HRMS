using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeLanguageConfiguration
    : IEntityTypeConfiguration<EmployeeLanguage>
{
    public void Configure(
        EntityTypeBuilder<EmployeeLanguage> builder)
    {
        builder.ToTable("EmployeeLanguages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.LanguageId)
               .IsRequired();

        builder.Property(x => x.ProficiencyLevel)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.CanRead)
               .IsRequired();

        builder.Property(x => x.CanWrite)
               .IsRequired();

        builder.Property(x => x.CanSpeak)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Language>()
               .WithMany()
               .HasForeignKey(x => x.LanguageId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.LanguageId);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.LanguageId
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
