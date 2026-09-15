using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.States.Queries.GetStates;

public sealed class GetStatesQueryValidator
    : AbstractValidator<GetStatesQuery>
{
    private static readonly string[] AllowedSortFields =
    {
        "Code",
        "Name",
        "IsActive"
    };

    public GetStatesQueryValidator()
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
