using FluentValidation;

namespace HRMS.Application.Features.EmploymentTypes.Commands.DeleteEmploymentType;

public class DeleteEmploymentTypeCommandValidator
    : AbstractValidator<DeleteEmploymentTypeCommand>
{
    public DeleteEmploymentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
