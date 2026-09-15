using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.DeleteState;

public sealed class DeleteStateCommandValidator
    : AbstractValidator<DeleteStateCommand>
{
    public DeleteStateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
