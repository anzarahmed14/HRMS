using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.CreateDocumentType;

public class CreateDocumentTypeCommandValidator
    : AbstractValidator<CreateDocumentTypeCommand>
{
    public CreateDocumentTypeCommandValidator()
    {
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
