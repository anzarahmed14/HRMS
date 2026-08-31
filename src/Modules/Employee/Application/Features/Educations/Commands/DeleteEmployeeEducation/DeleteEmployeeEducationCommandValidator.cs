using FluentValidation;

namespace HRMS.Application.Features.Educations.Commands.DeleteEmployeeEducation;

public class DeleteEmployeeEducationCommandValidator
    : AbstractValidator<DeleteEmployeeEducationCommand>
{
    public DeleteEmployeeEducationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
