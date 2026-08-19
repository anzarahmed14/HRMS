using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class LeavePolicyRuleConfiguration
    : IEntityTypeConfiguration<LeavePolicyRule>
{
    public void Configure(EntityTypeBuilder<LeavePolicyRule> builder)
    {
        builder.ToTable("LeavePolicyRules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AnnualEntitlement)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasIndex(x => new
        {
            x.LeavePolicyId,
            x.LeaveTypeId
        })
        .IsUnique();

        builder.HasOne<LeavePolicy>()
            .WithMany()
            .HasForeignKey(x => x.LeavePolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<LeaveType>()
            .WithMany()
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}