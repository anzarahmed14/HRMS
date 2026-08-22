using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class AttendanceRecordConfiguration
    : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(
        EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.AttendanceShiftId)
            .IsRequired();

        builder.Property(x => x.AttendancePolicyId)
            .IsRequired();

        builder.Property(x => x.AttendanceDate)
            .IsRequired();

        builder.Property(x => x.CheckIn);

        builder.Property(x => x.CheckOut);

        builder.Property(x => x.WorkedMinutes)
            .IsRequired();

        builder.Property(x => x.LateMinutes)
            .IsRequired();

        builder.Property(x => x.EarlyLeaveMinutes)
            .IsRequired();

        builder.Property(x => x.OvertimeMinutes)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.AttendanceDate
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");
    }
}
