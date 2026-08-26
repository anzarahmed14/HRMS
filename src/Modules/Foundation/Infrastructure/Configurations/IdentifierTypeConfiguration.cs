using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class IdentifierTypeConfiguration
    : IEntityTypeConfiguration<IdentifierType>
{
    private static readonly Guid PanId =
        Guid.Parse("44000000-0000-0000-0000-000000000001");

    private static readonly Guid UanId =
        Guid.Parse("44000000-0000-0000-0000-000000000002");

    private static readonly Guid PfId =
        Guid.Parse("44000000-0000-0000-0000-000000000003");

    private static readonly Guid EsicId =
        Guid.Parse("44000000-0000-0000-0000-000000000004");

    private static readonly Guid AadhaarId =
        Guid.Parse("44000000-0000-0000-0000-000000000005");

    private static readonly Guid PassportId =
        Guid.Parse("44000000-0000-0000-0000-000000000006");

    private static readonly Guid DrivingLicenseId =
        Guid.Parse("44000000-0000-0000-0000-000000000007");

    public void Configure(
        EntityTypeBuilder<IdentifierType> builder)
    {
        builder.ToTable("IdentifierTypes");

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

        builder.Property(x => x.IsSensitive)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasIndex(x => x.Code)
               .IsUnique();

        builder.HasIndex(x => x.Name)
               .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(
            new IdentifierType
            {
                Id = PanId,
                Code = "PAN",
                Name = "Permanent Account Number",
                Description = "Indian PAN identifier.",
                IsSensitive = true,
                IsActive = true
            },
            new IdentifierType
            {
                Id = UanId,
                Code = "UAN",
                Name = "Universal Account Number",
                Description = "Employee UAN.",
                IsSensitive = true,
                IsActive = true
            },
            new IdentifierType
            {
                Id = PfId,
                Code = "PF",
                Name = "Provident Fund Number",
                Description = "Employee provident fund identifier.",
                IsSensitive = true,
                IsActive = true
            },
            new IdentifierType
            {
                Id = EsicId,
                Code = "ESIC",
                Name = "ESIC Number",
                Description = "Employee ESIC identifier.",
                IsSensitive = true,
                IsActive = true
            },
            new IdentifierType
            {
                Id = AadhaarId,
                Code = "AADHAAR",
                Name = "Aadhaar Number",
                Description = "Indian Aadhaar identifier.",
                IsSensitive = true,
                IsActive = true
            },
            new IdentifierType
            {
                Id = PassportId,
                Code = "PASSPORT",
                Name = "Passport Number",
                Description = "Passport identifier.",
                IsSensitive = true,
                IsActive = true
            },
            new IdentifierType
            {
                Id = DrivingLicenseId,
                Code = "DRIVING_LICENSE",
                Name = "Driving License Number",
                Description = "Driving license identifier.",
                IsSensitive = true,
                IsActive = true
            });
    }
}
