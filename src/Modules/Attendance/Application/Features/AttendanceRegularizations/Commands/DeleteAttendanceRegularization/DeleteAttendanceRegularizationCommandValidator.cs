using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.DeleteAttendanceRegularization;

public sealed class DeleteAttendanceRegularizationCommandValidator
    : AbstractValidator<DeleteAttendanceRegularizationCommand>
{
    public DeleteAttendanceRegularizationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Regularization ID is required.");
    }
}
