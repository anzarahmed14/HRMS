using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.DeleteLanguage;

public class DeleteLanguageCommandValidator
    : AbstractValidator<DeleteLanguageCommand>
{
    public DeleteLanguageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
