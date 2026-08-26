using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class MaritalStatusConfiguration
    : IEntityTypeConfiguration<MaritalStatus>
{
    private static readonly Guid SingleId =
        Guid.Parse("46000000-0000-0000-0000-000000000001");

    private static readonly Guid MarriedId =
        Guid.Parse("46000000-0000-0000-0000-000000000002");

    private static readonly Guid DivorcedId =
        Guid.Parse("46000000-0000-0000-0000-000000000003");

    private static readonly Guid WidowedId =
        Guid.Parse("46000000-0000-0000-0000-000000000004");

    private static readonly Guid SeparatedId =
        Guid.Parse("46000000-0000-0000-0000-000000000005");

    private static readonly Guid PreferNotToSayId =
        Guid.Parse("46000000-0000-0000-0000-000000000006");

    public void Configure(
        EntityTypeBuilder<MaritalStatus> builder)
    {
        builder.ToTable("MaritalStatuses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasIndex(x => x.Code)
               .IsUnique();

        builder.HasIndex(x => x.Name)
               .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(
            new MaritalStatus
            {
                Id = SingleId,
                Code = "SINGLE",
                Name = "Single",
                Description = "Single",
                IsActive = true
            },
            new MaritalStatus
            {
                Id = MarriedId,
                Code = "MARRIED",
                Name = "Married",
                Description = "Married",
                IsActive = true
            },
            new MaritalStatus
            {
                Id = DivorcedId,
                Code = "DIVORCED",
                Name = "Divorced",
                Description = "Divorced",
                IsActive = true
            },
            new MaritalStatus
            {
                Id = WidowedId,
                Code = "WIDOWED",
                Name = "Widowed",
                Description = "Widowed",
                IsActive = true
            },
            new MaritalStatus
            {
                Id = SeparatedId,
                Code = "SEPARATED",
                Name = "Separated",
                Description = "Separated",
                IsActive = true
            },
            new MaritalStatus
            {
                Id = PreferNotToSayId,
                Code = "PREFER_NOT_TO_SAY",
                Name = "Prefer Not To Say",
                Description = "Employee prefers not to disclose.",
                IsActive = true
            });
    }
}
