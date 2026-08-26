using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class AddressTypeConfiguration
    : IEntityTypeConfiguration<AddressType>
{
    private static readonly Guid CurrentAddressTypeId =
        Guid.Parse("40000000-0000-0000-0000-000000000001");

    private static readonly Guid PermanentAddressTypeId =
        Guid.Parse("40000000-0000-0000-0000-000000000002");

    private static readonly Guid TemporaryAddressTypeId =
        Guid.Parse("40000000-0000-0000-0000-000000000003");

    public void Configure(
        EntityTypeBuilder<AddressType> builder)
    {
        builder.ToTable("AddressTypes");

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
            new AddressType
            {
                Id = CurrentAddressTypeId,
                Code = "CURRENT",
                Name = "Current",
                Description = "Current residential address",
                IsActive = true
            },
            new AddressType
            {
                Id = PermanentAddressTypeId,
                Code = "PERMANENT",
                Name = "Permanent",
                Description = "Permanent residential address",
                IsActive = true
            },
            new AddressType
            {
                Id = TemporaryAddressTypeId,
                Code = "TEMPORARY",
                Name = "Temporary",
                Description = "Temporary residential address",
                IsActive = true
            });
    }
}
