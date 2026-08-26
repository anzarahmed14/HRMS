using MediatR;

namespace HRMS.Application.Features.BankAccounts.Commands.UpdateBankAccount;

public record UpdateBankAccountCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public string AccountHolderName { get; init; } = string.Empty;

    public string AccountNumber { get; init; } = string.Empty;

    public string BankName { get; init; } = string.Empty;

    public string IFSCCode { get; init; } = string.Empty;

    public string? BranchName { get; init; }

    public string AccountType { get; init; } = string.Empty;

    public bool IsPrimary { get; init; }
}
