using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class RelationshipConfiguration
    : IEntityTypeConfiguration<Relationship>
{
    private static readonly Guid SpouseId =
        Guid.Parse("43000000-0000-0000-0000-000000000001");

    private static readonly Guid FatherId =
        Guid.Parse("43000000-0000-0000-0000-000000000002");

    private static readonly Guid MotherId =
        Guid.Parse("43000000-0000-0000-0000-000000000003");

    private static readonly Guid BrotherId =
        Guid.Parse("43000000-0000-0000-0000-000000000004");

    private static readonly Guid SisterId =
        Guid.Parse("43000000-0000-0000-0000-000000000005");

    private static readonly Guid SonId =
        Guid.Parse("43000000-0000-0000-0000-000000000006");

    private static readonly Guid DaughterId =
        Guid.Parse("43000000-0000-0000-0000-000000000007");

    private static readonly Guid OtherId =
        Guid.Parse("43000000-0000-0000-0000-000000000008");

    public void Configure(
        EntityTypeBuilder<Relationship> builder)
    {
        builder.ToTable("Relationships");

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
            new Relationship
            {
                Id = SpouseId,
                Code = "SPOUSE",
                Name = "Spouse",
                Description = "Spouse",
                IsActive = true
            },
            new Relationship
            {
                Id = FatherId,
                Code = "FATHER",
                Name = "Father",
                Description = "Father",
                IsActive = true
            },
            new Relationship
            {
                Id = MotherId,
                Code = "MOTHER",
                Name = "Mother",
                Description = "Mother",
                IsActive = true
            },
            new Relationship
            {
                Id = BrotherId,
                Code = "BROTHER",
                Name = "Brother",
                Description = "Brother",
                IsActive = true
            },
            new Relationship
            {
                Id = SisterId,
                Code = "SISTER",
                Name = "Sister",
                Description = "Sister",
                IsActive = true
            },
            new Relationship
            {
                Id = SonId,
                Code = "SON",
                Name = "Son",
                Description = "Son",
                IsActive = true
            },
            new Relationship
            {
                Id = DaughterId,
                Code = "DAUGHTER",
                Name = "Daughter",
                Description = "Daughter",
                IsActive = true
            },
            new Relationship
            {
                Id = OtherId,
                Code = "OTHER",
                Name = "Other",
                Description = "Other relationship",
                IsActive = true
            });
    }
}
