using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.UpdateMaritalStatus;

public class UpdateMaritalStatusCommandValidator
    : AbstractValidator<UpdateMaritalStatusCommand>
{
    public UpdateMaritalStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(250);
    }
}
