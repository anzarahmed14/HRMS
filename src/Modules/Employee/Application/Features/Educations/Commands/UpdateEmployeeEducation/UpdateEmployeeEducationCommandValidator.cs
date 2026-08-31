using FluentValidation;

namespace HRMS.Application.Features.Educations.Commands.UpdateEmployeeEducation;

public class UpdateEmployeeEducationCommandValidator
    : AbstractValidator<UpdateEmployeeEducationCommand>
{
    public UpdateEmployeeEducationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.EducationLevel)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Qualification)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Specialization)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Specialization));

        RuleFor(x => x.InstitutionName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.UniversityName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.UniversityName));

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.Grade)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Grade));
    }
}
