using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdatePermission;

public class UpdatePermissionCommandValidator
    : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
