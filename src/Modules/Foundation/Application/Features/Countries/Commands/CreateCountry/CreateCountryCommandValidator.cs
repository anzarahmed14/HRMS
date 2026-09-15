using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.CreateCountry;

public sealed class CreateCountryCommandValidator
    : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
