using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.ChangePassword;

public class ChangePasswordCommandValidator
    : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage(
                "New password must contain at least 8 characters.");

        RuleFor(x => x)
            .Must(x => x.CurrentPassword != x.NewPassword)
            .WithMessage(
                "New password must be different from the current password.");
    }
}
