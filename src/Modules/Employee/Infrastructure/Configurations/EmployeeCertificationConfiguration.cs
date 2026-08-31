using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeCertificationConfiguration
    : IEntityTypeConfiguration<EmployeeCertification>
{
    public void Configure(
        EntityTypeBuilder<EmployeeCertification> builder)
    {
        builder.ToTable("EmployeeCertifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.CertificationId)
               .IsRequired();

        builder.Property(x => x.CertificationNumber)
               .HasMaxLength(100);

        builder.Property(x => x.IssueDate)
               .IsRequired();

        builder.Property(x => x.ExpiryDate);

        builder.Property(x => x.CredentialUrl)
               .HasMaxLength(500);

        builder.Property(x => x.IsVerified)
               .IsRequired();

        builder.Property(x => x.VerifiedOn);

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Certification>()
               .WithMany()
               .HasForeignKey(x => x.CertificationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.CertificationId);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.CertificationId
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
