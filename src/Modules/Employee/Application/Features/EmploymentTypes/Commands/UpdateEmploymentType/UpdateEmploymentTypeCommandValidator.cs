using FluentValidation;

namespace HRMS.Application.Features.EmploymentTypes.Commands.UpdateEmploymentType;

public class UpdateEmploymentTypeCommandValidator
    : AbstractValidator<UpdateEmploymentTypeCommand>
{
    public UpdateEmploymentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
