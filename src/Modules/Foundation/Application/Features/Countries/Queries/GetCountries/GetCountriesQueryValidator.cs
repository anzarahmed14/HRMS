using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Countries.Queries.GetCountries;

public sealed class GetCountriesQueryValidator
    : AbstractValidator<GetCountriesQuery>
{
    private static readonly string[] AllowedSortFields =
    {
        "Code",
        "Name",
        "IsActive"
    };

    public GetCountriesQueryValidator()
    {
        RuleFor(x => x.Request.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.Request.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Request.SortBy)
            .Must(x => string.IsNullOrWhiteSpace(x) ||
                       AllowedSortFields.Contains(
                           x,
                           StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                $"SortBy must be one of: {string.Join(", ", AllowedSortFields)}.");
    }
}
