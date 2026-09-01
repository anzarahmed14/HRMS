using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivatePermission;

public class DeactivatePermissionCommandValidator
    : AbstractValidator<DeactivatePermissionCommand>
{
    public DeactivatePermissionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
