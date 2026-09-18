using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.DeleteAttendanceDayStatus;

public sealed class DeleteAttendanceDayStatusCommandValidator
    : AbstractValidator<DeleteAttendanceDayStatusCommand>
{
    public DeleteAttendanceDayStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance day status ID is required.");
    }
}
