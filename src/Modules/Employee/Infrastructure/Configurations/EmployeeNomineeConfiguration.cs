using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmployeeNomineeConfiguration
    : IEntityTypeConfiguration<EmployeeNominee>
{
    public void Configure(
        EntityTypeBuilder<EmployeeNominee> builder)
    {
        builder.ToTable("EmployeeNominees");

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

        builder.Property(x => x.DateOfBirth)
               .IsRequired();

        builder.Property(x => x.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(x => x.Email)
               .HasMaxLength(200);

        builder.Property(x => x.IsMinor)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .IsRequired();

        // Employee ? EmployeeNominee
        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Relationship ? EmployeeNominee
        builder.HasOne<Relationship>()
               .WithMany()
               .HasForeignKey(x => x.RelationshipId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.RelationshipId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
