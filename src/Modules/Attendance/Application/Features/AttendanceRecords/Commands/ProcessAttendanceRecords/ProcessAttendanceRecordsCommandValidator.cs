using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.ProcessAttendanceRecords;

public sealed class ProcessAttendanceRecordsCommandValidator
    : AbstractValidator<ProcessAttendanceRecordsCommand>
{
    public ProcessAttendanceRecordsCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

        RuleFor(x => x.FromDate)
            .NotEmpty();

        RuleFor(x => x.ToDate)
            .NotEmpty();

        RuleFor(x => x)
            .Must(x => x.ToDate >= x.FromDate)
            .WithMessage("To date cannot be earlier than From date.");

        RuleFor(x => x)
            .Must(x => x.ToDate.DayNumber - x.FromDate.DayNumber <= 31)
            .WithMessage("Maximum processing range is 31 days.");
    }
}
