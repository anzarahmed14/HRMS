using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.CreateAttendanceDayStatus;

public sealed class CreateAttendanceDayStatusCommandValidator
    : AbstractValidator<CreateAttendanceDayStatusCommand>
{
    public CreateAttendanceDayStatusCommandValidator()
    {
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
