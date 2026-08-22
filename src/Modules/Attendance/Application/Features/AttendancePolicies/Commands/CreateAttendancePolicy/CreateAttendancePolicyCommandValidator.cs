using FluentValidation;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.CreateAttendancePolicy;

public class CreateAttendancePolicyCommandValidator
    : AbstractValidator<CreateAttendancePolicyCommand>
{
    public CreateAttendancePolicyCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

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
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MinimumOvertimeMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MaximumOvertimeMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.EffectiveTo)
            .GreaterThanOrEqualTo(x => x.EffectiveFrom)
            .When(x => x.EffectiveTo.HasValue);
    }
}
