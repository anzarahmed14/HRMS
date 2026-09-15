using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.UpdateSkill;

public class UpdateSkillCommandValidator
    : AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
