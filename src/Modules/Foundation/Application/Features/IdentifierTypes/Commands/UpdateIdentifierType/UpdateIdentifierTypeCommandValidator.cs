using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.UpdateIdentifierType;

public class UpdateIdentifierTypeCommandValidator
    : AbstractValidator<UpdateIdentifierTypeCommand>
{
    public UpdateIdentifierTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
