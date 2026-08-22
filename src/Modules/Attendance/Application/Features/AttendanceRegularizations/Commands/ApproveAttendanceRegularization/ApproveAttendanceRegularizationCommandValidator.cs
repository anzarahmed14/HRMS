using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.ApproveAttendanceRegularization;

public sealed class ApproveAttendanceRegularizationCommandValidator
    : AbstractValidator<ApproveAttendanceRegularizationCommand>
{
    public ApproveAttendanceRegularizationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Regularization ID is required.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000)
            .When(x => x.Remarks is not null);
    }
}
