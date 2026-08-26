using FluentValidation;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.CreateGovernmentIdentifier;

public class CreateGovernmentIdentifierCommandValidator
    : AbstractValidator<CreateGovernmentIdentifierCommand>
{
    public CreateGovernmentIdentifierCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.IdentifierTypeId)
            .NotEmpty();

        RuleFor(x => x.IdentifierNumber)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.IssueDate)
            .When(x => x.IssueDate.HasValue && x.ExpiryDate.HasValue);
    }
}
