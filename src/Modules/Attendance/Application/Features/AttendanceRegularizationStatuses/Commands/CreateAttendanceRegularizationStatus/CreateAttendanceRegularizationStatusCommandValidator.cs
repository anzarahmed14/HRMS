using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.CreateAttendanceRegularizationStatus;

public sealed class CreateAttendanceRegularizationStatusCommandValidator
    : AbstractValidator<CreateAttendanceRegularizationStatusCommand>
{
    public CreateAttendanceRegularizationStatusCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
