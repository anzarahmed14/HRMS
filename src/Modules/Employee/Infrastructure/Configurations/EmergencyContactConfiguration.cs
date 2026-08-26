using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class EmergencyContactConfiguration
    : IEntityTypeConfiguration<EmergencyContact>
{
    public void Configure(
        EntityTypeBuilder<EmergencyContact> builder)
    {
        builder.ToTable("EmergencyContacts");

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

        builder.Property(x => x.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.AlternatePhoneNumber)
               .HasMaxLength(20);

        builder.Property(x => x.Email)
               .HasMaxLength(200);

        builder.Property(x => x.IsPrimary)
               .IsRequired();

        // Employee ? EmergencyContact
        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Relationship ? EmergencyContact
        builder.HasOne<Relationship>()
               .WithMany()
               .HasForeignKey(x => x.RelationshipId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.RelationshipId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
