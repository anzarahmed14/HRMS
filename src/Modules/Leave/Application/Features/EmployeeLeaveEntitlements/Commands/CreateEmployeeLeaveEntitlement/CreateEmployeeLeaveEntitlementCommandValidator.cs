using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.CreateEmployeeLeaveEntitlement;

public class CreateEmployeeLeaveEntitlementCommandValidator
    : AbstractValidator<CreateEmployeeLeaveEntitlementCommand>
{
    public CreateEmployeeLeaveEntitlementCommandValidator()
    {
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