using FluentValidation;

namespace HRMS.Application.Features.Languages.Commands.DeleteEmployeeLanguage;

public class DeleteEmployeeLanguageCommandValidator
    : AbstractValidator<DeleteEmployeeLanguageCommand>
{
    public DeleteEmployeeLanguageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
