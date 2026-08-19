using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.UpdateEmployeeLeaveEntitlement;

public class UpdateEmployeeLeaveEntitlementCommandValidator
    : AbstractValidator<UpdateEmployeeLeaveEntitlementCommand>
{
    public UpdateEmployeeLeaveEntitlementCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.LeaveYearId)
            .NotEmpty();

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty();

        RuleFor(x => x.LeavePolicyRuleId)
            .NotEmpty();

        RuleFor(x => x.EntitledDays)
            .GreaterThanOrEqualTo(0);
    }
}
