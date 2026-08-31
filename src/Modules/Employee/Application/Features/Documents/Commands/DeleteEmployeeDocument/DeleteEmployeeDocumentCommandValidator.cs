using FluentValidation;

namespace HRMS.Application.Features.Documents.Commands.DeleteEmployeeDocument;

public class DeleteEmployeeDocumentCommandValidator
    : AbstractValidator<DeleteEmployeeDocumentCommand>
{
    public DeleteEmployeeDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
