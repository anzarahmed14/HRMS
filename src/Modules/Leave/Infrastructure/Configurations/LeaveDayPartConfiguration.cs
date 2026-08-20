using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class LeaveDayPartConfiguration
    : IEntityTypeConfiguration<LeaveDayPart>
{
    public void Configure(EntityTypeBuilder<LeaveDayPart> builder)
    {
        builder.ToTable("LeaveDayParts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DaysValue)
            .IsRequired()
            .HasPrecision(4, 2);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasData(
            new LeaveDayPart
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                Code = "FULL_DAY",
                Name = "Full Day",
                DaysValue = 1.00m,
                IsActive = true
            },
            new LeaveDayPart
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                Code = "FIRST_HALF",
                Name = "First Half",
                DaysValue = 0.50m,
                IsActive = true
            },
            new LeaveDayPart
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                Code = "SECOND_HALF",
                Name = "Second Half",
                DaysValue = 0.50m,
                IsActive = true
            });
    }
}
