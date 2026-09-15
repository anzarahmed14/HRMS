using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Queries.GetCountryById;

public sealed class GetCountryByIdQueryValidator
    : AbstractValidator<GetCountryByIdQuery>
{
    public GetCountryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
