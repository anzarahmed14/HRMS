using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeDependentConfiguration
    : IEntityTypeConfiguration<EmployeeDependent>
{
    public void Configure(
        EntityTypeBuilder<EmployeeDependent> builder)
    {
        builder.ToTable("EmployeeDependents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.RelationshipId)
               .IsRequired();

        builder.Property(x => x.GenderId)
               .IsRequired();

        builder.Property(x => x.DateOfBirth)
               .IsRequired();

        builder.Property(x => x.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(x => x.Email)
               .HasMaxLength(200);

        builder.Property(x => x.IsDependent)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .IsRequired();

        // Employee ? EmployeeDependent
        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Relationship ? EmployeeDependent
        builder.HasOne<Relationship>()
               .WithMany()
               .HasForeignKey(x => x.RelationshipId)
               .OnDelete(DeleteBehavior.Restrict);

        // Gender ? EmployeeDependent
        builder.HasOne<Gender>()
               .WithMany()
               .HasForeignKey(x => x.GenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.RelationshipId);

        builder.HasIndex(x => x.GenderId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
