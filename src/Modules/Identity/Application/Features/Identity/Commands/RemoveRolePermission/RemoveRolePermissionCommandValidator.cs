using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveRolePermission;

public class RemoveRolePermissionCommandValidator
    : AbstractValidator<RemoveRolePermissionCommand>
{
    public RemoveRolePermissionCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.PermissionId)
            .NotEmpty();
    }
}
