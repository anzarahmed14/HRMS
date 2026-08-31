using FluentValidation;

namespace HRMS.Application.Features.Experiences.Commands.CreateEmployeeExperience;

public class CreateEmployeeExperienceCommandValidator
    : AbstractValidator<CreateEmployeeExperienceCommand>
{
    public CreateEmployeeExperienceCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.JobTitle)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.EmploymentType)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.Location)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Location));

        RuleFor(x => x.Responsibilities)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Responsibilities));
    }
}
