namespace HRMS.Application.Features.BankAccounts.DTOs;

public class BankAccountDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string AccountHolderName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string MaskedAccountNumber { get; set; } = string.Empty;

    public string BankName { get; set; } = string.Empty;

    public string IFSCCode { get; set; } = string.Empty;

    public string? BranchName { get; set; }

    public string AccountType { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
}
