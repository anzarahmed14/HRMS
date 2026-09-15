using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.CreateIdentifierType;

public class CreateIdentifierTypeCommandValidator
    : AbstractValidator<CreateIdentifierTypeCommand>
{
    public CreateIdentifierTypeCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
