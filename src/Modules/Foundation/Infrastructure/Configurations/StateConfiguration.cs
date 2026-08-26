using HRMS.Modules.Foundation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Foundation.Infrastructure.Configurations;

public class StateConfiguration
    : IEntityTypeConfiguration<State>
{
    private static readonly Guid MaharashtraId =
        Guid.Parse("42000000-0000-0000-0000-000000000001");

    private static readonly Guid IndiaId =
        Guid.Parse("41000000-0000-0000-0000-000000000001");

    public void Configure(
        EntityTypeBuilder<State> builder)
    {
        builder.ToTable("States");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.CountryId)
               .IsRequired();

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.HasOne<Country>()
               .WithMany()
               .HasForeignKey(x => x.CountryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CountryId,
            x.Code
        })
        .IsUnique();

        builder.HasIndex(x => new
        {
            x.CountryId,
            x.Name
        })
        .IsUnique();

        builder.HasIndex(x => x.CountryId);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(
            new State
            {
                Id = MaharashtraId,
                CountryId = IndiaId,
                Code = "MH",
                Name = "Maharashtra",
                IsActive = true
            });
    }
}
