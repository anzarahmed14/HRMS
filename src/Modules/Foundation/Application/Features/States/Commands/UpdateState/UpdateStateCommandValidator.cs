using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.UpdateState;

public sealed class UpdateStateCommandValidator
    : AbstractValidator<UpdateStateCommand>
{
    public UpdateStateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CountryId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
