using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.UpdateAttendanceRecord;

public sealed class UpdateAttendanceRecordCommandValidator
    : AbstractValidator<UpdateAttendanceRecordCommand>
{
    public UpdateAttendanceRecordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance record ID is required.");

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
