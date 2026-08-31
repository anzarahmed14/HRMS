using FluentValidation;

namespace HRMS.Application.Features.Skills.Commands.DeleteEmployeeSkill;

public class DeleteEmployeeSkillCommandValidator
    : AbstractValidator<DeleteEmployeeSkillCommand>
{
    public DeleteEmployeeSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
