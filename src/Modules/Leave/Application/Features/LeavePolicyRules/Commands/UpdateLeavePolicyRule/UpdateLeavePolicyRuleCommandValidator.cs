using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.UpdateLeavePolicyRule;

public class UpdateLeavePolicyRuleCommandValidator
    : AbstractValidator<UpdateLeavePolicyRuleCommand>
{
    public UpdateLeavePolicyRuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.LeavePolicyId)
            .NotEmpty();

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty();

        RuleFor(x => x.AnnualEntitlement)
            .GreaterThanOrEqualTo(0);
    }
}
