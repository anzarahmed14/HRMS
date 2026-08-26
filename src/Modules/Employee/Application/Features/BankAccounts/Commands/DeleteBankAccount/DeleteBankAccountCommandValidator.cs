using FluentValidation;

namespace HRMS.Application.Features.BankAccounts.Commands.DeleteBankAccount;

public class DeleteBankAccountCommandValidator
    : AbstractValidator<DeleteBankAccountCommand>
{
    public DeleteBankAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
