using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class EmployeeLeaveEntitlementConfiguration
    : IEntityTypeConfiguration<EmployeeLeaveEntitlement>
{
    public void Configure(EntityTypeBuilder<EmployeeLeaveEntitlement> builder)
    {
        builder.ToTable("EmployeeLeaveEntitlements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntitledDays)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.LeaveYearId,
            x.LeaveTypeId
        })
        .IsUnique();

        builder.HasOne<LeaveYear>()
            .WithMany()
            .HasForeignKey(x => x.LeaveYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveType>()
            .WithMany()
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeavePolicyRule>()
            .WithMany()
            .HasForeignKey(x => x.LeavePolicyRuleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}