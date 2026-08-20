using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Queries.GetLeaveYears;

public class GetLeaveYearsQueryValidator
    : AbstractValidator<GetLeaveYearsQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "Code",
        "Name",
        "StartDate",
        "EndDate"
    ];

    public GetLeaveYearsQueryValidator()
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
