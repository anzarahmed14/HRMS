using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.RejectAttendanceRegularization;

public sealed class RejectAttendanceRegularizationCommandValidator
    : AbstractValidator<RejectAttendanceRegularizationCommand>
{
    public RejectAttendanceRegularizationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Regularization ID is required.");

        RuleFor(x => x.Remarks)
            .NotEmpty()
            .WithMessage("Rejection remarks are required.")
            .MaximumLength(1000);
    }
}
