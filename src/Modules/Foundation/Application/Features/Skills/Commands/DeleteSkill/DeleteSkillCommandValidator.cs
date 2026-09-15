using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Skills.Commands.DeleteSkill;

public class DeleteSkillCommandValidator
    : AbstractValidator<DeleteSkillCommand>
{
    public DeleteSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
