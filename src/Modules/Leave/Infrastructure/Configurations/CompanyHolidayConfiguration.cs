using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public sealed class CompanyHolidayConfiguration
    : IEntityTypeConfiguration<CompanyHoliday>
{
    public void Configure(
        EntityTypeBuilder<CompanyHoliday> builder)
    {
        builder.ToTable("CompanyHolidays");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HolidayDate)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.HolidayType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IsOptional)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasOne(x => x.LeaveYear)
            .WithMany()
            .HasForeignKey(x => x.LeaveYearId)
            .OnDelete(DeleteBehavior.Restrict);

        // One holiday date can exist only once
        // within a Leave Year.
        builder.HasIndex(x => new
        {
            x.LeaveYearId,
            x.HolidayDate
        })
        .IsUnique();
    }
}
