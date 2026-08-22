using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.CreateAttendanceRawLog;

public sealed class CreateAttendanceRawLogCommandValidator
    : AbstractValidator<CreateAttendanceRawLogCommand>
{
    public CreateAttendanceRawLogCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

        RuleFor(x => x.AttendanceDeviceId)
            .NotEmpty()
            .WithMessage("Attendance device is required.");

        RuleFor(x => x.PunchDateTime)
            .NotEmpty()
            .WithMessage("Punch date/time is required.");

        RuleFor(x => x.PunchType)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.ExternalRecordId)
            .MaximumLength(200);
    }
}
