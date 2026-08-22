using HRMS.Modules.Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Attendance.Infrastructure.Configurations;

public class AttendanceSourceConfiguration
    : IEntityTypeConfiguration<AttendanceSource>
{
    public void Configure(
        EntityTypeBuilder<AttendanceSource> builder)
    {
        builder.ToTable("AttendanceSources");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SourceType)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.Code
        })
        .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
