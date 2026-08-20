using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequests;

public sealed class GetLeaveRequestsQueryValidator
    : AbstractValidator<GetLeaveRequestsQuery>
{
    private const int MaxPageSize = 100;

    private static readonly string[] AllowedSortFields =
    [
        "FromDate",
        "ToDate",
        "TotalDays",
        "AppliedOn"
    ];

    public GetLeaveRequestsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage($"Page size must be between 1 and {MaxPageSize}.");

        RuleFor(x => x.SortBy)
            .Must(BeAllowedSortField)
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage(
                $"SortBy must be one of: {string.Join(", ", AllowedSortFields)}.");
    }

    private static bool BeAllowedSortField(string? sortBy)
    {
        return AllowedSortFields.Contains(
            sortBy!,
            StringComparer.OrdinalIgnoreCase);
    }
}
