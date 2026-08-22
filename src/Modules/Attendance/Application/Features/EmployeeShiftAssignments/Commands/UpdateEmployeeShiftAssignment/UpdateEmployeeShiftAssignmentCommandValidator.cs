using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.UpdateEmployeeShiftAssignment;

public sealed class UpdateEmployeeShiftAssignmentCommandValidator
    : AbstractValidator<UpdateEmployeeShiftAssignmentCommand>
{
    public UpdateEmployeeShiftAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Employee shift assignment ID is required.");

        RuleFor(x => x.AttendanceShiftId)
            .NotEmpty()
            .WithMessage("Attendance shift is required.");

        RuleFor(x => x.AttendancePolicyId)
            .NotEmpty()
            .WithMessage("Attendance policy is required.");

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty()
            .WithMessage("Effective From date is required.");

        RuleFor(x => x)
            .Must(x =>
                !x.EffectiveTo.HasValue ||
                x.EffectiveTo.Value >= x.EffectiveFrom)
            .WithMessage(
                "Effective To date cannot be earlier than Effective From date.");
    }
}
