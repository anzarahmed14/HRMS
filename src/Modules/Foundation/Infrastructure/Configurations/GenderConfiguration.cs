using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class GenderConfiguration
    : IEntityTypeConfiguration<Gender>
{
    private static readonly Guid MaleId =
        Guid.Parse("45000000-0000-0000-0000-000000000001");

    private static readonly Guid FemaleId =
        Guid.Parse("45000000-0000-0000-0000-000000000002");

    private static readonly Guid NonBinaryId =
        Guid.Parse("45000000-0000-0000-0000-000000000003");

    private static readonly Guid OtherId =
        Guid.Parse("45000000-0000-0000-0000-000000000004");

    private static readonly Guid PreferNotToSayId =
        Guid.Parse("45000000-0000-0000-0000-000000000005");

    public void Configure(
        EntityTypeBuilder<Gender> builder)
    {
        builder.ToTable("Genders");

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
            new Gender
            {
                Id = MaleId,
                Code = "MALE",
                Name = "Male",
                Description = "Male",
                IsActive = true
            },
            new Gender
            {
                Id = FemaleId,
                Code = "FEMALE",
                Name = "Female",
                Description = "Female",
                IsActive = true
            },
            new Gender
            {
                Id = NonBinaryId,
                Code = "NON_BINARY",
                Name = "Non-Binary",
                Description = "Non-binary",
                IsActive = true
            },
            new Gender
            {
                Id = OtherId,
                Code = "OTHER",
                Name = "Other",
                Description = "Other",
                IsActive = true
            },
            new Gender
            {
                Id = PreferNotToSayId,
                Code = "PREFER_NOT_TO_SAY",
                Name = "Prefer Not To Say",
                Description = "Employee prefers not to disclose.",
                IsActive = true
            });
    }
}
