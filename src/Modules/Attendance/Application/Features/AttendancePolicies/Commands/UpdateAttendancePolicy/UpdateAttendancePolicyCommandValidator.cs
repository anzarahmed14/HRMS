using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.UpdateAttendancePolicy;

public sealed class UpdateAttendancePolicyCommandValidator
    : AbstractValidator<UpdateAttendancePolicyCommand>
{
    public UpdateAttendancePolicyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance policy ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.GracePeriodMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MinimumWorkingMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.FullDayMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.HalfDayMinutes)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.FullDayMinutes)
            .WithMessage(
                "Half day minutes cannot be greater than full day minutes.");

        RuleFor(x => x.MinimumOvertimeMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MaximumOvertimeMinutes)
            .GreaterThanOrEqualTo(x => x.MinimumOvertimeMinutes)
            .WithMessage(
                "Maximum overtime minutes cannot be less than minimum overtime minutes.");

        RuleFor(x => x.EffectiveTo)
            .GreaterThanOrEqualTo(x => x.EffectiveFrom)
            .When(x => x.EffectiveTo.HasValue)
            .WithMessage(
                "Effective to date cannot be earlier than effective from date.");
    }
}
