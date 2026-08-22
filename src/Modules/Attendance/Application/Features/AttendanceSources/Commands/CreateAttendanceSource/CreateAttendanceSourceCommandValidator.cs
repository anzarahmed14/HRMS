using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.CreateAttendanceSource;

public sealed class CreateAttendanceSourceCommandValidator
    : AbstractValidator<CreateAttendanceSourceCommand>
{
    public CreateAttendanceSourceCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company is required.");

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
