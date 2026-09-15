using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.DeleteIdentifierType;

public class DeleteIdentifierTypeCommandValidator
    : AbstractValidator<DeleteIdentifierTypeCommand>
{
    public DeleteIdentifierTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
