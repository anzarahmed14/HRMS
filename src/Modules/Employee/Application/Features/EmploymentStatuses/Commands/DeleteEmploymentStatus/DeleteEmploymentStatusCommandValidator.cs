using FluentValidation;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.DeleteEmploymentStatus;

public class DeleteEmploymentStatusCommandValidator
    : AbstractValidator<DeleteEmploymentStatusCommand>
{
    public DeleteEmploymentStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
