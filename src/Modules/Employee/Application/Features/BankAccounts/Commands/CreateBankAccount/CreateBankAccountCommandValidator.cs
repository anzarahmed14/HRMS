using FluentValidation;

namespace HRMS.Application.Features.BankAccounts.Commands.CreateBankAccount;

public class CreateBankAccountCommandValidator
    : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.AccountHolderName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.IFSCCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.BranchName)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.BranchName));

        RuleFor(x => x.AccountType)
            .NotEmpty()
            .MaximumLength(30);
    }
}
