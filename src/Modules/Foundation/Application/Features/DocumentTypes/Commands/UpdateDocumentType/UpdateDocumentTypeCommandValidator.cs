using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.UpdateDocumentType;

public class UpdateDocumentTypeCommandValidator
    : AbstractValidator<UpdateDocumentTypeCommand>
{
    public UpdateDocumentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(40);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
