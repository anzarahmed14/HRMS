using FluentValidation;

namespace HRMS.Application.Features.Languages.Commands.CreateEmployeeLanguage;

public class CreateEmployeeLanguageCommandValidator
    : AbstractValidator<CreateEmployeeLanguageCommand>
{
    public CreateEmployeeLanguageCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.LanguageId)
            .NotEmpty();

        RuleFor(x => x.ProficiencyLevel)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x)
            .Must(x =>
                x.CanRead ||
                x.CanWrite ||
                x.CanSpeak)
            .WithMessage(
                "At least one language capability must be selected.");
    }
}
