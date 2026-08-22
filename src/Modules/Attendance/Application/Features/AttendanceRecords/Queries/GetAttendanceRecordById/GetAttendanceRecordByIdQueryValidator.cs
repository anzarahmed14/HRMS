using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecordById;

public sealed class GetAttendanceRecordByIdQueryValidator
    : AbstractValidator<GetAttendanceRecordByIdQuery>
{
    public GetAttendanceRecordByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance record ID is required.");
    }
}
