using FluentValidation;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.VerifyGovernmentIdentifier;

public class VerifyGovernmentIdentifierCommandValidator
    : AbstractValidator<VerifyGovernmentIdentifierCommand>
{
    public VerifyGovernmentIdentifierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
