using FluentValidation;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.DeleteGovernmentIdentifier;

public class DeleteGovernmentIdentifierCommandValidator
    : AbstractValidator<DeleteGovernmentIdentifierCommand>
{
    public DeleteGovernmentIdentifierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
