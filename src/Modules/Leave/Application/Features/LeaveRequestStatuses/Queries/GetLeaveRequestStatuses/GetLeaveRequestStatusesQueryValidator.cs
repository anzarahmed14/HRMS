using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatuses;

public class GetLeaveRequestStatusesQueryValidator
    : AbstractValidator<GetLeaveRequestStatusesQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "Code",
        "Name",
        "IsActive"
    ];

    public GetLeaveRequestStatusesQueryValidator()
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
