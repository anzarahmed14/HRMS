using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.ResetPassword;

public class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage(
                "New password must contain at least 8 characters.");
    }
}