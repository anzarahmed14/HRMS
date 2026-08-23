using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public sealed class AttendanceDayStatusConfiguration
    : IEntityTypeConfiguration<AttendanceDayStatus>
{
    public void Configure(
        EntityTypeBuilder<AttendanceDayStatus> builder)
    {
        builder.ToTable("AttendanceDayStatuses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasData(
            new AttendanceDayStatus
            {
                Id = Guid.Parse(
                    "11000000-0000-0000-0000-000000000001"),
                Code = "WORKING_DAY",
                Name = "Working Day",
                Description = "Normal working day.",
                IsActive = true
            },
            new AttendanceDayStatus
            {
                Id = Guid.Parse(
                    "11000000-0000-0000-0000-000000000002"),
                Code = "WEEKLY_OFF",
                Name = "Weekly Off",
                Description = "Scheduled weekly day off.",
                IsActive = true
            },
            new AttendanceDayStatus
            {
                Id = Guid.Parse(
                    "11000000-0000-0000-0000-000000000003"),
                Code = "HOLIDAY",
                Name = "Holiday",
                Description = "Company holiday.",
                IsActive = true
            },
            new AttendanceDayStatus
            {
                Id = Guid.Parse(
                    "11000000-0000-0000-0000-000000000004"),
                Code = "LEAVE",
                Name = "Leave",
                Description = "Employee has approved leave.",
                IsActive = true
            });
    }
}