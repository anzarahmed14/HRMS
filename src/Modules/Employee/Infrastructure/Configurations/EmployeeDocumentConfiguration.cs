using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeDocumentConfiguration
    : IEntityTypeConfiguration<EmployeeDocument>
{
    public void Configure(
        EntityTypeBuilder<EmployeeDocument> builder)
    {
        builder.ToTable("EmployeeDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.DocumentTypeId)
               .IsRequired();

        builder.Property(x => x.DocumentName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.StorageKey)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.FileSize)
               .IsRequired();

        builder.Property(x => x.UploadedOn)
               .IsRequired();

        builder.Property(x => x.IsVerified)
               .IsRequired();

        builder.Property(x => x.VerifiedOn);

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DocumentType>()
               .WithMany()
               .HasForeignKey(x => x.DocumentTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.DocumentTypeId);

        builder.HasIndex(x => x.StorageKey)
               .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
