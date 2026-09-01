using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveUserRole;

public class RemoveUserRoleCommandValidator
    : AbstractValidator<RemoveUserRoleCommand>
{
    public RemoveUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
