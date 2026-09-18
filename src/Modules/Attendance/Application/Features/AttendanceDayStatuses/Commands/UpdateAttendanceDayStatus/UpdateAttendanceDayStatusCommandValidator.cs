using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.UpdateAttendanceDayStatus;

public sealed class UpdateAttendanceDayStatusCommandValidator
    : AbstractValidator<UpdateAttendanceDayStatusCommand>
{
    public UpdateAttendanceDayStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance day status ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(250);
    }
}
