using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class LeaveRequestStatusConfiguration
    : IEntityTypeConfiguration<LeaveRequestStatus>
{
    public void Configure(EntityTypeBuilder<LeaveRequestStatus> builder)
    {
        builder.ToTable("LeaveRequestStatuses");

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
            new LeaveRequestStatus
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                Code = "PENDING",
                Name = "Pending",
                IsActive = true
            },
            new LeaveRequestStatus
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                Code = "APPROVED",
                Name = "Approved",
                IsActive = true
            },
            new LeaveRequestStatus
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                Code = "REJECTED",
                Name = "Rejected",
                IsActive = true
            },
            new LeaveRequestStatus
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                Code = "CANCELLED",
                Name = "Cancelled",
                IsActive = true
            });
    }
}
