using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.ImportAttendanceRawLogs;

public sealed class ImportAttendanceRawLogsCommandValidator
    : AbstractValidator<ImportAttendanceRawLogsCommand>
{
    public ImportAttendanceRawLogsCommandValidator()
    {
        RuleFor(x => x.Records)
            .NotEmpty()
            .WithMessage("At least one attendance raw log is required.")
            .Must(x => x.Count <= 1000)
            .WithMessage("A maximum of 1000 records can be imported at once.");

        RuleForEach(x => x.Records)
            .SetValidator(new ImportAttendanceRawLogItemValidator());
    }
}

public sealed class ImportAttendanceRawLogItemValidator
    : AbstractValidator<ImportAttendanceRawLogItem>
{
    public ImportAttendanceRawLogItemValidator()
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
