using FluentValidation;

namespace HRMS.Application.Features.Dependents.Commands.DeleteEmployeeDependent;

public class DeleteEmployeeDependentCommandValidator
    : AbstractValidator<DeleteEmployeeDependentCommand>
{
    public DeleteEmployeeDependentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
