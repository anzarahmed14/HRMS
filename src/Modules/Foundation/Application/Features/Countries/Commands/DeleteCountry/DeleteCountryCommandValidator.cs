using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Commands.DeleteCountry;

public sealed class DeleteCountryCommandValidator
    : AbstractValidator<DeleteCountryCommand>
{
    public DeleteCountryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
