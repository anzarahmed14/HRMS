using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.UpdateAttendanceShift;

public sealed class UpdateAttendanceShiftCommandValidator
    : AbstractValidator<UpdateAttendanceShiftCommand>
{
    public UpdateAttendanceShiftCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance shift ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.BreakMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.EffectiveTo)
            .GreaterThanOrEqualTo(x => x.EffectiveFrom)
            .When(x => x.EffectiveTo.HasValue)
            .WithMessage(
                "Effective to date cannot be earlier than effective from date.");
    }
}
