using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignRolePermission;

public class AssignRolePermissionCommandValidator
    : AbstractValidator<AssignRolePermissionCommand>
{
    public AssignRolePermissionCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.PermissionId)
            .NotEmpty();
    }
}
