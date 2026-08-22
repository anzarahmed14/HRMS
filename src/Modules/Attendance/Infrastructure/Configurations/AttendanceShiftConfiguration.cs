using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class AttendanceShiftConfiguration : IEntityTypeConfiguration<AttendanceShift>
{
    public void Configure(EntityTypeBuilder<AttendanceShift> builder)
    {
        builder.ToTable("AttendanceShifts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();

        builder.Property(x => x.BreakMinutes)
            .IsRequired();

        builder.Property(x => x.IsOvernight)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.EffectiveFrom)
            .IsRequired();

        builder.Property(x => x.EffectiveTo);

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.HasOne<HRMS.Modules.Companies.Domain.Entities.Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.Code
        })
        .IsUnique();
    }
}
