using FluentValidation;

namespace HRMS.Application.Features.EmploymentTypes.Commands.CreateEmploymentType;

public class CreateEmploymentTypeCommandValidator
    : AbstractValidator<CreateEmploymentTypeCommand>
{
    public CreateEmploymentTypeCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
