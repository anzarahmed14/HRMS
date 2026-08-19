using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.DeleteLeavePolicyRule;

public class DeleteLeavePolicyRuleCommandValidator
    : AbstractValidator<DeleteLeavePolicyRuleCommand>
{
    public DeleteLeavePolicyRuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
