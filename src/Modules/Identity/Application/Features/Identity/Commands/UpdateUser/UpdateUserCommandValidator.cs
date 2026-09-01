using FluentValidation;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdateUser;

public class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x)
            .NotNull();
    }
}
