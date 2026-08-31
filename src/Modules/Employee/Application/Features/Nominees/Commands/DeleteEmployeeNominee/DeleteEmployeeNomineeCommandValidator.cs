using FluentValidation;

namespace HRMS.Application.Features.Nominees.Commands.DeleteEmployeeNominee;

public class DeleteEmployeeNomineeCommandValidator
    : AbstractValidator<DeleteEmployeeNomineeCommand>
{
    public DeleteEmployeeNomineeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
