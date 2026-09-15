using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.CreateSkill;

public class CreateSkillCommandValidator
    : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
