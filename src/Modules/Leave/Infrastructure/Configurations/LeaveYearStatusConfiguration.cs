using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class LeaveYearStatusConfiguration
    : IEntityTypeConfiguration<LeaveYearStatus>
{
    public void Configure(EntityTypeBuilder<LeaveYearStatus> builder)
    {
        builder.ToTable("LeaveYearStatuses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasData(
    new LeaveYearStatus
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
        Code = "DRAFT",
        Name = "Draft",
        IsActive = true,
        CreatedOn = new DateTimeOffset(
            2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
    },
    new LeaveYearStatus
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
        Code = "ACTIVE",
        Name = "Active",
        IsActive = true,
        CreatedOn = new DateTimeOffset(
            2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
    },
    new LeaveYearStatus
    {
        Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
        Code = "CLOSED",
        Name = "Closed",
        IsActive = true,
        CreatedOn = new DateTimeOffset(
            2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
    });
    }
}