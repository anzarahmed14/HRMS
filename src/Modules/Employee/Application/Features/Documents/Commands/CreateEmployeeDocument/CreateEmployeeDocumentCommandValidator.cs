using FluentValidation;

namespace HRMS.Application.Features.Documents.Commands.CreateEmployeeDocument;

public class CreateEmployeeDocumentCommandValidator
    : AbstractValidator<CreateEmployeeDocumentCommand>
{
    public CreateEmployeeDocumentCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.DocumentTypeId)
            .NotEmpty();

        RuleFor(x => x.DocumentName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.StorageKey)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.FileSize)
            .GreaterThan(0);
    }
}
