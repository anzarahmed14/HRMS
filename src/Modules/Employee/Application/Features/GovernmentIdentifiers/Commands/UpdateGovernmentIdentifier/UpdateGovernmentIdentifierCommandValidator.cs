using FluentValidation;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.UpdateGovernmentIdentifier;

public class UpdateGovernmentIdentifierCommandValidator
    : AbstractValidator<UpdateGovernmentIdentifierCommand>
{
    public UpdateGovernmentIdentifierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
