using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.DeleteLeavePolicy;

public class DeleteLeavePolicyCommandValidator
    : AbstractValidator<DeleteLeavePolicyCommand>
{
    public DeleteLeavePolicyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
