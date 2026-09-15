using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypes;

public sealed class GetAddressTypesQueryValidator
    : AbstractValidator<GetAddressTypesQuery>
{
    private static readonly string[] AllowedSortFields =
    {
        "Code",
        "Name",
        "IsActive"
    };

    public GetAddressTypesQueryValidator()
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
