using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.DeleteDocumentType;

public class DeleteDocumentTypeCommandValidator
    : AbstractValidator<DeleteDocumentTypeCommand>
{
    public DeleteDocumentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
