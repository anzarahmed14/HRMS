using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.UpdateAttendanceRegularization;

public sealed class UpdateAttendanceRegularizationCommandValidator
    : AbstractValidator<UpdateAttendanceRegularizationCommand>
{
    public UpdateAttendanceRegularizationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Regularization ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Regularization reason is required.")
            .MaximumLength(1000);

        RuleFor(x => x)
            .Must(x =>
                x.RequestedCheckIn.HasValue ||
                x.RequestedCheckOut.HasValue)
            .WithMessage(
                "Check-in or check-out is required.");

        RuleFor(x => x)
            .Must(x =>
                !x.RequestedCheckIn.HasValue ||
                !x.RequestedCheckOut.HasValue ||
                x.RequestedCheckOut.Value > x.RequestedCheckIn.Value)
            .WithMessage(
                "Check-out must be later than check-in.");
    }
}
