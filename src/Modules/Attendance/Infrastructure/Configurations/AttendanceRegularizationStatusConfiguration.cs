using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public sealed class AttendanceRegularizationStatusConfiguration
    : IEntityTypeConfiguration<AttendanceRegularizationStatus>
{
    public void Configure(
        EntityTypeBuilder<AttendanceRegularizationStatus> builder)
    {
        builder.ToTable("AttendanceRegularizationStatuses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasData(
            new AttendanceRegularizationStatus
            {
                Id = Guid.Parse(
                    "10000000-0000-0000-0000-000000000101"),
                Code = "PENDING",
                Name = "Pending",
                IsActive = true
            },
            new AttendanceRegularizationStatus
            {
                Id = Guid.Parse(
                    "10000000-0000-0000-0000-000000000102"),
                Code = "APPROVED",
                Name = "Approved",
                IsActive = true
            },
            new AttendanceRegularizationStatus
            {
                Id = Guid.Parse(
                    "10000000-0000-0000-0000-000000000103"),
                Code = "REJECTED",
                Name = "Rejected",
                IsActive = true
            },
            new AttendanceRegularizationStatus
            {
                Id = Guid.Parse(
                    "10000000-0000-0000-0000-000000000104"),
                Code = "CANCELLED",
                Name = "Cancelled",
                IsActive = true
            });
    }
}