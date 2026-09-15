using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatuses;

public class GetLeaveYearStatusesQueryValidator
    : AbstractValidator<GetLeaveYearStatusesQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "Code",
        "Name",
        "IsActive"
    ];

    public GetLeaveYearStatusesQueryValidator()
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
