using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreatePermission;

public class CreatePermissionCommandValidator
    : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
