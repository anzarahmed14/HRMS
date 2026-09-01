using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateRole;

public class DeactivateRoleCommandValidator
    : AbstractValidator<DeactivateRoleCommand>
{
    public DeactivateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
