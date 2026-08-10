using FluentValidation;

namespace HRMS.Application.Features.Identity.Commands.CreateUser;

public class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee Id is required.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must contain at least 8 characters.");
    }
}