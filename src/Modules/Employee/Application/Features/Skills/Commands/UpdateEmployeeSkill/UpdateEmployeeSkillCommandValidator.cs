using FluentValidation;

namespace HRMS.Application.Features.Skills.Commands.UpdateEmployeeSkill;

public class UpdateEmployeeSkillCommandValidator
    : AbstractValidator<UpdateEmployeeSkillCommand>
{
    public UpdateEmployeeSkillCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SkillId).NotEmpty();

        RuleFor(x => x.ProficiencyLevel)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(60);
    }
}
