using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypes;

public class GetIdentifierTypesQueryValidator
    : AbstractValidator<GetIdentifierTypesQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "Code",
        "Name",
        "IsSensitive",
        "IsActive"
    ];

    public GetIdentifierTypesQueryValidator()
    {
        RuleFor(x => x.Request.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.Request.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Request.SortBy)
            .Must(sortBy =>
                string.IsNullOrWhiteSpace(sortBy) ||
                AllowedSortFields.Contains(
                    sortBy,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                $"SortBy must be one of: {string.Join(", ", AllowedSortFields)}.");
    }
}
