using HRMS.Modules.Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Modules.Employee.Infrastructure.Configurations;

public class BankAccountConfiguration
    : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(
        EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("EmployeeBankAccounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
               .IsRequired();

        builder.Property(x => x.AccountHolderName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.AccountNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.BankName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.IFSCCode)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.BranchName)
               .HasMaxLength(150);

        builder.Property(x => x.AccountType)
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(x => x.IsPrimary)
               .IsRequired();

        builder.HasOne<HRMS.Modules.Employee.Domain.Entities.Employee>()
               .WithMany()
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => x.EmployeeId)
               .IsUnique()
               .HasFilter("[IsPrimary] = 1 AND [IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
