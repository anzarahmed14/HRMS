using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class CertificationConfiguration
    : IEntityTypeConfiguration<Certification>
{
    private static readonly Guid AzureAdministratorId =
        Guid.Parse("48000000-0000-0000-0000-000000000001");

    private static readonly Guid AzureDeveloperId =
        Guid.Parse("48000000-0000-0000-0000-000000000002");

    private static readonly Guid AwsDeveloperId =
        Guid.Parse("48000000-0000-0000-0000-000000000003");

    private static readonly Guid AwsSolutionsArchitectId =
        Guid.Parse("48000000-0000-0000-0000-000000000004");

    private static readonly Guid CsharpCertificationId =
        Guid.Parse("48000000-0000-0000-0000-000000000005");

    private static readonly Guid JavaCertificationId =
        Guid.Parse("48000000-0000-0000-0000-000000000006");

    private static readonly Guid PmpId =
        Guid.Parse("48000000-0000-0000-0000-000000000007");

    private static readonly Guid ScrumMasterId =
        Guid.Parse("48000000-0000-0000-0000-000000000008");

    public void Configure(
        EntityTypeBuilder<Certification> builder)
    {
        builder.ToTable("Certifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.IssuingOrganization)
               .HasMaxLength(150);

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
            new Certification
            {
                Id = AzureAdministratorId,
                Code = "AZURE_ADMINISTRATOR",
                Name = "Microsoft Certified: Azure Administrator Associate",
                IssuingOrganization = "Microsoft",
                IsActive = true
            },
            new Certification
            {
                Id = AzureDeveloperId,
                Code = "AZURE_DEVELOPER",
                Name = "Microsoft Certified: Azure Developer Associate",
                IssuingOrganization = "Microsoft",
                IsActive = true
            },
            new Certification
            {
                Id = AwsDeveloperId,
                Code = "AWS_DEVELOPER",
                Name = "AWS Certified Developer",
                IssuingOrganization = "Amazon Web Services",
                IsActive = true
            },
            new Certification
            {
                Id = AwsSolutionsArchitectId,
                Code = "AWS_SOLUTIONS_ARCHITECT",
                Name = "AWS Certified Solutions Architect",
                IssuingOrganization = "Amazon Web Services",
                IsActive = true
            },
            new Certification
            {
                Id = CsharpCertificationId,
                Code = "CSHARP",
                Name = "C# Certification",
                IssuingOrganization = "Microsoft",
                IsActive = true
            },
            new Certification
            {
                Id = JavaCertificationId,
                Code = "JAVA",
                Name = "Java Certification",
                IssuingOrganization = "Oracle",
                IsActive = true
            },
            new Certification
            {
                Id = PmpId,
                Code = "PMP",
                Name = "Project Management Professional",
                IssuingOrganization = "PMI",
                IsActive = true
            },
            new Certification
            {
                Id = ScrumMasterId,
                Code = "SCRUM_MASTER",
                Name = "Certified ScrumMaster",
                IssuingOrganization = "Scrum Alliance",
                IsActive = true
            });
    }
}
