using FluentValidation;

namespace HRMS.Application.Features.Documents.Commands.UpdateEmployeeDocument;

public class UpdateEmployeeDocumentCommandValidator
    : AbstractValidator<UpdateEmployeeDocumentCommand>
{
    public UpdateEmployeeDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
