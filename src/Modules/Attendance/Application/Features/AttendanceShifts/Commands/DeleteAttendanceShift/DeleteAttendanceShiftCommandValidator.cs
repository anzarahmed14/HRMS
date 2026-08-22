using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.DeleteAttendanceShift;

public sealed class DeleteAttendanceShiftCommandValidator
    : AbstractValidator<DeleteAttendanceShiftCommand>
{
    public DeleteAttendanceShiftCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance shift ID is required.");
    }
}
