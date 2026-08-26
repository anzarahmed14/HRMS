using FluentValidation;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.CreateEmploymentStatus;

public class CreateEmploymentStatusCommandValidator
    : AbstractValidator<CreateEmploymentStatusCommand>
{
    public CreateEmploymentStatusCommandValidator()
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
