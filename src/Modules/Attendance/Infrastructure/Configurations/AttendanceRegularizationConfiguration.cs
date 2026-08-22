using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public sealed class AttendanceRegularizationConfiguration
    : IEntityTypeConfiguration<AttendanceRegularization>
{
    public void Configure(
        EntityTypeBuilder<AttendanceRegularization> builder)
    {
        builder.ToTable("AttendanceRegularizations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.AttendanceRecordId)
            .IsRequired();

        builder.Property(x => x.AttendanceRegularizationStatusId)
            .IsRequired();

        builder.Property(x => x.AttendanceDate)
            .IsRequired();

        builder.Property(x => x.RequestedCheckIn);

        builder.Property(x => x.RequestedCheckOut);

        builder.Property(x => x.Reason)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.RequestedBy);

        builder.Property(x => x.RequestedOn)
            .IsRequired();

        builder.Property(x => x.ApprovedBy);

        builder.Property(x => x.ApprovedOn);

        builder.Property(x => x.ApprovalRemarks)
            .HasMaxLength(1000);

        builder.Property(x => x.RejectedBy);

        builder.Property(x => x.RejectedOn);

        builder.Property(x => x.RejectionRemarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CancelledBy);

        builder.Property(x => x.CancelledOn);

        builder.Property(x => x.CancellationRemarks)
            .HasMaxLength(1000);

        // AttendanceRegularization -> AttendanceRegularizationStatus
        builder.HasOne<AttendanceRegularizationStatus>()
            .WithMany()
            .HasForeignKey(x => x.AttendanceRegularizationStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.AttendanceDate
        });

        builder.HasIndex(x => x.AttendanceRegularizationStatusId);

        builder.HasIndex(x => x.AttendanceRecordId);
    }
}