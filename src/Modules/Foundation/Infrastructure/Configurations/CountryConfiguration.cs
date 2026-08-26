using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class CountryConfiguration
    : IEntityTypeConfiguration<Country>
{
    private static readonly Guid IndiaId =
        Guid.Parse("41000000-0000-0000-0000-000000000001");

    private static readonly Guid UnitedStatesId =
        Guid.Parse("41000000-0000-0000-0000-000000000002");

    private static readonly Guid UnitedKingdomId =
        Guid.Parse("41000000-0000-0000-0000-000000000003");

    private static readonly Guid UnitedArabEmiratesId =
        Guid.Parse("41000000-0000-0000-0000-000000000004");

    public void Configure(
        EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasIndex(x => x.Code)
               .IsUnique();

        builder.HasIndex(x => x.Name)
               .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(
            new Country
            {
                Id = IndiaId,
                Code = "IN",
                Name = "India",
                IsActive = true
            },
            new Country
            {
                Id = UnitedStatesId,
                Code = "US",
                Name = "United States",
                IsActive = true
            },
            new Country
            {
                Id = UnitedKingdomId,
                Code = "GB",
                Name = "United Kingdom",
                IsActive = true
            },
            new Country
            {
                Id = UnitedArabEmiratesId,
                Code = "AE",
                Name = "United Arab Emirates",
                IsActive = true
            });
    }
}
