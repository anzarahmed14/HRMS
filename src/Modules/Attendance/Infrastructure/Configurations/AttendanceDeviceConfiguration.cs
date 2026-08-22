using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class AttendanceDeviceConfiguration
    : IEntityTypeConfiguration<AttendanceDevice>
{
    public void Configure(
        EntityTypeBuilder<AttendanceDevice> builder)
    {
        builder.ToTable("AttendanceDevices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.AttendanceSourceId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(100);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(50);

        builder.Property(x => x.Location)
            .HasMaxLength(200);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.AttendanceSourceId,
            x.Code
        })
        .IsUnique();

        builder.HasIndex(x => x.SerialNumber)
            .IsUnique()
            .HasFilter("[SerialNumber] IS NOT NULL");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
