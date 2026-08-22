using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.UpdateAttendanceSource;

public sealed class UpdateAttendanceSourceCommandValidator
    : AbstractValidator<UpdateAttendanceSourceCommand>
{
    public UpdateAttendanceSourceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance source ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.SourceType)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
