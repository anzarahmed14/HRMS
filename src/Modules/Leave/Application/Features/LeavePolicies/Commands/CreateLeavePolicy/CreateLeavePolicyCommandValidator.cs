using FluentValidation;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.CreateLeavePolicy;

public class CreateLeavePolicyCommandValidator
    : AbstractValidator<CreateLeavePolicyCommand>
{
    public CreateLeavePolicyCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}