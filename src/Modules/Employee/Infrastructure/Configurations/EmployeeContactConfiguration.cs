using HRMS.Modules.Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeContactConfiguration
    : IEntityTypeConfiguration<EmployeeContact>
{
    public void Configure(
        EntityTypeBuilder<EmployeeContact> builder)
    {
        builder.ToTable("EmployeeContacts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.ContactType)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(x => x.ContactValue)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.IsPrimary)
               .IsRequired();

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.ContactType
        });

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
