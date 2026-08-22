using HRMS.Modules.Attendance.Domain.Entities;
using HRMS.Modules.Companies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class AttendancePolicyConfiguration : IEntityTypeConfiguration<AttendancePolicy>
{
    public void Configure(EntityTypeBuilder<AttendancePolicy> builder)
    {
        builder.ToTable("AttendancePolicies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.GracePeriodMinutes)
            .IsRequired();

        builder.Property(x => x.MinimumWorkingMinutes)
            .IsRequired();

        builder.Property(x => x.FullDayMinutes)
            .IsRequired();

        builder.Property(x => x.HalfDayMinutes)
            .IsRequired();

        builder.Property(x => x.IsOvertimeAllowed)
            .IsRequired();

        builder.Property(x => x.MinimumOvertimeMinutes)
            .IsRequired();

        builder.Property(x => x.MaximumOvertimeMinutes)
            .IsRequired();

        builder.Property(x => x.OvertimeRequiresApproval)
            .IsRequired();

        builder.Property(x => x.IsDefault)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.EffectiveFrom)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.Code
        })
        .IsUnique();

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
