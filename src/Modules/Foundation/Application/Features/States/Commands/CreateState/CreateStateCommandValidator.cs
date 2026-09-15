using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.CreateState;

public sealed class CreateStateCommandValidator
    : AbstractValidator<CreateStateCommand>
{
    public CreateStateCommandValidator()
    {
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
