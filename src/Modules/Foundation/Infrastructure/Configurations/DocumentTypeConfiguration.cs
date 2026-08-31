using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class DocumentTypeConfiguration
    : IEntityTypeConfiguration<DocumentType>
{
    private static readonly Guid PanCardId =
        Guid.Parse("47000000-0000-0000-0000-000000000001");

    private static readonly Guid AadhaarCardId =
        Guid.Parse("47000000-0000-0000-0000-000000000002");

    private static readonly Guid PassportId =
        Guid.Parse("47000000-0000-0000-0000-000000000003");

    private static readonly Guid DrivingLicenseId =
        Guid.Parse("47000000-0000-0000-0000-000000000004");

    private static readonly Guid AddressProofId =
        Guid.Parse("47000000-0000-0000-0000-000000000005");

    private static readonly Guid OfferLetterId =
        Guid.Parse("47000000-0000-0000-0000-000000000006");

    private static readonly Guid JoiningLetterId =
        Guid.Parse("47000000-0000-0000-0000-000000000007");

    private static readonly Guid ExperienceLetterId =
        Guid.Parse("47000000-0000-0000-0000-000000000008");

    private static readonly Guid EducationCertificateId =
        Guid.Parse("47000000-0000-0000-0000-000000000009");

    private static readonly Guid OtherId =
        Guid.Parse("47000000-0000-0000-0000-000000000010");

    public void Configure(
        EntityTypeBuilder<DocumentType> builder)
    {
        builder.ToTable("DocumentTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(40);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

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
            new DocumentType
            {
                Id = PanCardId,
                Code = "PAN_CARD",
                Name = "PAN Card",
                Description = "Permanent Account Number document.",
                IsSensitive = true,
                IsActive = true
            },
            new DocumentType
            {
                Id = AadhaarCardId,
                Code = "AADHAAR_CARD",
                Name = "Aadhaar Card",
                Description = "Aadhaar identity document.",
                IsSensitive = true,
                IsActive = true
            },
            new DocumentType
            {
                Id = PassportId,
                Code = "PASSPORT",
                Name = "Passport",
                Description = "Passport document.",
                IsSensitive = true,
                IsActive = true
            },
            new DocumentType
            {
                Id = DrivingLicenseId,
                Code = "DRIVING_LICENSE",
                Name = "Driving License",
                Description = "Driving license document.",
                IsSensitive = true,
                IsActive = true
            },
            new DocumentType
            {
                Id = AddressProofId,
                Code = "ADDRESS_PROOF",
                Name = "Address Proof",
                Description = "Proof of residential address.",
                IsSensitive = false,
                IsActive = true
            },
            new DocumentType
            {
                Id = OfferLetterId,
                Code = "OFFER_LETTER",
                Name = "Offer Letter",
                Description = "Employee offer letter.",
                IsSensitive = false,
                IsActive = true
            },
            new DocumentType
            {
                Id = JoiningLetterId,
                Code = "JOINING_LETTER",
                Name = "Joining Letter",
                Description = "Employee joining letter.",
                IsSensitive = false,
                IsActive = true
            },
            new DocumentType
            {
                Id = ExperienceLetterId,
                Code = "EXPERIENCE_LETTER",
                Name = "Experience Letter",
                Description = "Previous employment experience letter.",
                IsSensitive = false,
                IsActive = true
            },
            new DocumentType
            {
                Id = EducationCertificateId,
                Code = "EDUCATION_CERTIFICATE",
                Name = "Education Certificate",
                Description = "Educational qualification certificate.",
                IsSensitive = false,
                IsActive = true
            },
            new DocumentType
            {
                Id = OtherId,
                Code = "OTHER",
                Name = "Other",
                Description = "Other employee document.",
                IsSensitive = false,
                IsActive = true
            });
    }
}
