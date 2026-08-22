using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.CreateEmployeeShiftAssignment;

public sealed class CreateEmployeeShiftAssignmentCommandValidator
    : AbstractValidator<CreateEmployeeShiftAssignmentCommand>
{
    public CreateEmployeeShiftAssignmentCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");

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
