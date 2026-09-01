using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateUser;

public class DeactivateUserCommandValidator
    : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
