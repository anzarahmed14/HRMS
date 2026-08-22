using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class EmployeeShiftAssignmentConfiguration
    : IEntityTypeConfiguration<EmployeeShiftAssignment>
{
    public void Configure(
        EntityTypeBuilder<EmployeeShiftAssignment> builder)
    {
        builder.ToTable("EmployeeShiftAssignments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.AttendanceShiftId)
            .IsRequired();

        builder.Property(x => x.AttendancePolicyId)
            .IsRequired();

        builder.Property(x => x.EffectiveFrom)
            .IsRequired();

        builder.Property(x => x.EffectiveTo)
            .IsRequired(false);

        builder.Property(x => x.IsPrimary)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.EmployeeId,
            x.EffectiveFrom
        });

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}