using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.UpdateAttendanceRegularizationStatus;

public sealed class UpdateAttendanceRegularizationStatusCommandValidator
    : AbstractValidator<UpdateAttendanceRegularizationStatusCommand>
{
    public UpdateAttendanceRegularizationStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance regularization status ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
