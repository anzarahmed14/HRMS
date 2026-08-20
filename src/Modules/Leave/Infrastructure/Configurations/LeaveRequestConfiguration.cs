
using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeEntity = HRMS.Modules.Employee.Domain.Entities.Employee;
namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class LeaveRequestConfiguration
    : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApprovalReason)
             .HasMaxLength(500);

        builder.Property(x => x.CancellationReason)
    .HasMaxLength(500);

        builder.Property(x => x.EmployeeId)
            .IsRequired();  

        builder.Property(x => x.LeaveYearId)
            .IsRequired();

        builder.Property(x => x.LeaveTypeId)
            .IsRequired();

        builder.Property(x => x.StartDayPartId)
            .IsRequired();

        builder.Property(x => x.EndDayPartId)
            .IsRequired();

        builder.Property(x => x.StatusId)
            .IsRequired();

        builder.Property(x => x.FromDate)
            .IsRequired();

        builder.Property(x => x.ToDate)
            .IsRequired();

        builder.Property(x => x.TotalDays)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.RejectionReason)
    .HasMaxLength(500);

        builder.Property(x => x.AppliedOn)
            .IsRequired();

        builder.HasOne<EmployeeEntity>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveYear>()
            .WithMany()
            .HasForeignKey(x => x.LeaveYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveType>()
            .WithMany()
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveDayPart>()
            .WithMany()
            .HasForeignKey(x => x.StartDayPartId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveDayPart>()
            .WithMany()
            .HasForeignKey(x => x.EndDayPartId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveRequestStatus>()
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.LeaveYearId,
            x.LeaveTypeId
        });

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.StatusId
        });

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.FromDate,
            x.ToDate
        });
    }
}
