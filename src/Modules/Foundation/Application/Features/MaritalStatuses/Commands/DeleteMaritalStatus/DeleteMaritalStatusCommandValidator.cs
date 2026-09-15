using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.DeleteMaritalStatus;

public class DeleteMaritalStatusCommandValidator
    : AbstractValidator<DeleteMaritalStatusCommand>
{
    public DeleteMaritalStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
