using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.CreateLanguage;

public class CreateLanguageCommandValidator
    : AbstractValidator<CreateLanguageCommand>
{
    public CreateLanguageCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
