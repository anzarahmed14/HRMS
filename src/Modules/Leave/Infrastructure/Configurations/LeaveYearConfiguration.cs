using HRMS.Modules.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Leave.Infrastructure.Configurations;

public class LeaveYearConfiguration
    : IEntityTypeConfiguration<LeaveYear>
{
    public void Configure(EntityTypeBuilder<LeaveYear> builder)
    {
        builder.ToTable("LeaveYears");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.StatusId)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.Code
        })
 .IsUnique()
 .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.Name
        })
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.StatusId
        });

        builder.HasOne<LeaveYearStatus>()
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}