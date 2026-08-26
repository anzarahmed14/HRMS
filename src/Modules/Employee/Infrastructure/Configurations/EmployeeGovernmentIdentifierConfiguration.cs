using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeGovernmentIdentifierConfiguration
    : IEntityTypeConfiguration<EmployeeGovernmentIdentifier>
{
    public void Configure(
        EntityTypeBuilder<EmployeeGovernmentIdentifier> builder)
    {
        builder.ToTable("EmployeeGovernmentIdentifiers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.IdentifierTypeId)
               .IsRequired();

        builder.Property(x => x.IdentifierNumber)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.IssueDate);

        builder.Property(x => x.ExpiryDate);

        builder.Property(x => x.IsVerified)
               .IsRequired();

        builder.Property(x => x.VerifiedOn);

        // Employee ? Government Identifier
        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // IdentifierType ? Government Identifier
        builder.HasOne<IdentifierType>()
               .WithMany()
               .HasForeignKey(x => x.IdentifierTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.IdentifierTypeId);

        // One active identifier of each type per employee
        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.IdentifierTypeId
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
