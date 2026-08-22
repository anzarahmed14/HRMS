using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.CreateManualAttendanceRecord;

public sealed class CreateManualAttendanceRecordCommandValidator
    : AbstractValidator<CreateManualAttendanceRecordCommand>
{
    public CreateManualAttendanceRecordCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

        RuleFor(x => x.AttendanceDate)
            .NotEmpty()
            .WithMessage("Attendance date is required.");

        RuleFor(x => x)
            .Must(x =>
                !x.CheckIn.HasValue ||
                !x.CheckOut.HasValue ||
                x.CheckOut.Value > x.CheckIn.Value)
            .WithMessage(
                "Check-out must be later than check-in.");

        RuleFor(x => x)
            .Must(x =>
                x.CheckIn.HasValue ||
                x.CheckOut.HasValue)
            .WithMessage(
                "Check-in or check-out is required.");

        RuleFor(x => x.Remarks)
            .MaximumLength(500);
    }
}
