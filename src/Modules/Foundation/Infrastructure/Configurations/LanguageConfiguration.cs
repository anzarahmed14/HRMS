using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class LanguageConfiguration
    : IEntityTypeConfiguration<Language>
{
    private static readonly Guid EnglishId =
        Guid.Parse("49000000-0000-0000-0000-000000000001");

    private static readonly Guid HindiId =
        Guid.Parse("49000000-0000-0000-0000-000000000002");

    private static readonly Guid MarathiId =
        Guid.Parse("49000000-0000-0000-0000-000000000003");

    private static readonly Guid UrduId =
        Guid.Parse("49000000-0000-0000-0000-000000000004");

    private static readonly Guid GujaratiId =
        Guid.Parse("49000000-0000-0000-0000-000000000005");

    private static readonly Guid BengaliId =
        Guid.Parse("49000000-0000-0000-0000-000000000006");

    private static readonly Guid TamilId =
        Guid.Parse("49000000-0000-0000-0000-000000000007");

    private static readonly Guid TeluguId =
        Guid.Parse("49000000-0000-0000-0000-000000000008");

    public void Configure(
        EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(20);

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
            new Language
            {
                Id = EnglishId,
                Code = "EN",
                Name = "English",
                IsActive = true
            },
            new Language
            {
                Id = HindiId,
                Code = "HI",
                Name = "Hindi",
                IsActive = true
            },
            new Language
            {
                Id = MarathiId,
                Code = "MR",
                Name = "Marathi",
                IsActive = true
            },
            new Language
            {
                Id = UrduId,
                Code = "UR",
                Name = "Urdu",
                IsActive = true
            },
            new Language
            {
                Id = GujaratiId,
                Code = "GU",
                Name = "Gujarati",
                IsActive = true
            },
            new Language
            {
                Id = BengaliId,
                Code = "BN",
                Name = "Bengali",
                IsActive = true
            },
            new Language
            {
                Id = TamilId,
                Code = "TA",
                Name = "Tamil",
                IsActive = true
            },
            new Language
            {
                Id = TeluguId,
                Code = "TE",
                Name = "Telugu",
                IsActive = true
            });
    }
}
