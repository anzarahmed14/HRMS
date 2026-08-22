using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.DeleteAttendanceSource;

public sealed class DeleteAttendanceSourceCommandValidator
    : AbstractValidator<DeleteAttendanceSourceCommand>
{
    public DeleteAttendanceSourceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance source ID is required.");
    }
}
