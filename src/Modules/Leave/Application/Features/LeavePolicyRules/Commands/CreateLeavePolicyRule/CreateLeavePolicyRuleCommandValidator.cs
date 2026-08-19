using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.CreateLeavePolicyRule;

public class CreateLeavePolicyRuleCommandValidator
    : AbstractValidator<CreateLeavePolicyRuleCommand>
{
    public CreateLeavePolicyRuleCommandValidator()
    {
        RuleFor(x => x.LeavePolicyId)
            .NotEmpty();

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty();

        RuleFor(x => x.AnnualEntitlement)
            .GreaterThanOrEqualTo(0);
    }
}