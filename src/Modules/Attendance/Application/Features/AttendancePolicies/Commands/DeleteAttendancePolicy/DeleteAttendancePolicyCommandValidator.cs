using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.DeleteAttendancePolicy;

public sealed class DeleteAttendancePolicyCommandValidator
    : AbstractValidator<DeleteAttendancePolicyCommand>
{
    public DeleteAttendancePolicyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance policy ID is required.");
    }
}
