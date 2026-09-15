using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.UpdateCountry;

public sealed class UpdateCountryCommandValidator
    : AbstractValidator<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
