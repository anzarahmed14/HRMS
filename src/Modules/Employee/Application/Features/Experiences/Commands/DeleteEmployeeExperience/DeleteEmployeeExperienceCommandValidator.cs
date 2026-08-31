using FluentValidation;

namespace HRMS.Application.Features.Experiences.Commands.DeleteEmployeeExperience;

public class DeleteEmployeeExperienceCommandValidator
    : AbstractValidator<DeleteEmployeeExperienceCommand>
{
    public DeleteEmployeeExperienceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
