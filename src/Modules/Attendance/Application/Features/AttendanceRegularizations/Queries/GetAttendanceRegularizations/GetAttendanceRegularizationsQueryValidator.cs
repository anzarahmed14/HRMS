using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizations;

public sealed class GetAttendanceRegularizationsQueryValidator
    : AbstractValidator<GetAttendanceRegularizationsQuery>
{
    public GetAttendanceRegularizationsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x)
            .Must(x =>
                !x.FromDate.HasValue ||
                !x.ToDate.HasValue ||
                x.FromDate.Value <= x.ToDate.Value)
            .WithMessage(
                "FromDate must be less than or equal to ToDate.");
    }
}
