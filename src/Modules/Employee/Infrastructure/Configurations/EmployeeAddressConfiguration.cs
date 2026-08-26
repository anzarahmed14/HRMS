using HRMS.Modules.Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeAddressConfiguration
    : IEntityTypeConfiguration<EmployeeAddress>
{
    public void Configure(
        EntityTypeBuilder<EmployeeAddress> builder)
    {
        builder.ToTable("EmployeeAddresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.AddressTypeId)
               .IsRequired();

        builder.Property(x => x.CountryId)
               .IsRequired();

        builder.Property(x => x.StateId)
               .IsRequired();

        builder.Property(x => x.AddressLine1)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(x => x.AddressLine2)
               .HasMaxLength(250);

        builder.Property(x => x.City)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.IsPrimary)
               .IsRequired();

        // Employee ? EmployeeAddress
        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // AddressType ? EmployeeAddress
        builder.HasOne<HRMS.Modules.Foundation.Domain.Entities.AddressType>()
               .WithMany()
               .HasForeignKey(x => x.AddressTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Country ? EmployeeAddress
        builder.HasOne<HRMS.Modules.Foundation.Domain.Entities.Country>()
               .WithMany()
               .HasForeignKey(x => x.CountryId)
               .OnDelete(DeleteBehavior.Restrict);

        // State ? EmployeeAddress
        builder.HasOne<HRMS.Modules.Foundation.Domain.Entities.State>()
               .WithMany()
               .HasForeignKey(x => x.StateId)
               .OnDelete(DeleteBehavior.Restrict);

        // Basic lookup indexes
        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.AddressTypeId);

        builder.HasIndex(x => x.CountryId);

        builder.HasIndex(x => x.StateId);

        // One active address of each type per employee
        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.AddressTypeId
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        // Only one active primary address per employee
        builder.HasIndex(x => x.EmployeeId)
               .IsUnique()
               .HasFilter("[IsPrimary] = 1 AND [IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
