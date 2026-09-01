using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignUserRole;

public class AssignUserRoleCommandValidator
    : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
