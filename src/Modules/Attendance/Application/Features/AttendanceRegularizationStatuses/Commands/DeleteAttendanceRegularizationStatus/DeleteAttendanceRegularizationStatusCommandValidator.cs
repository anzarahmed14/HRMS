using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.DeleteAttendanceRegularizationStatus;

public sealed class DeleteAttendanceRegularizationStatusCommandValidator
    : AbstractValidator<DeleteAttendanceRegularizationStatusCommand>
{
    public DeleteAttendanceRegularizationStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance regularization status ID is required.");
    }
}
