using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class AttendanceRawLogConfiguration
    : IEntityTypeConfiguration<AttendanceRawLog>
{
    public void Configure(
        EntityTypeBuilder<AttendanceRawLog> builder)
    {
        builder.ToTable("AttendanceRawLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.AttendanceDeviceId)
            .IsRequired();

        builder.Property(x => x.PunchDateTime)
            .IsRequired();

        builder.Property(x => x.PunchType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.ExternalRecordId)
            .HasMaxLength(200);

        builder.Property(x => x.RawData);

        builder.Property(x => x.ImportedOn)
            .IsRequired();

        builder.HasIndex(x => x.ExternalRecordId)
            .IsUnique()
            .HasFilter("[ExternalRecordId] IS NOT NULL");

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.PunchDateTime
        });

        builder.HasIndex(x => x.AttendanceDeviceId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
