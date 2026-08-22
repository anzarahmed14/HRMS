using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecords;

public sealed class GetAttendanceRecordsQueryValidator
    : AbstractValidator<GetAttendanceRecordsQuery>
{
    public GetAttendanceRecordsQueryValidator()
    {
        RuleFor(x => x.Page)
            .NotNull();

        RuleFor(x => x)
            .Must(x =>
                !x.FromDate.HasValue ||
                !x.ToDate.HasValue ||
                x.ToDate.Value >= x.FromDate.Value)
            .WithMessage(
                "To date cannot be earlier than From date.");
    }
}
