using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.CreateAttendanceRegularization;

public sealed class CreateAttendanceRegularizationCommandValidator
    : AbstractValidator<CreateAttendanceRegularizationCommand>
{
    public CreateAttendanceRegularizationCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

        RuleFor(x => x.AttendanceDate)
            .NotEmpty()
            .WithMessage("Attendance date is required.");

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
