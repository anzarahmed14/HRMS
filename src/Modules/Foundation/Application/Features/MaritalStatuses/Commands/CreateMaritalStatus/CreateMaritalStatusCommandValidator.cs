using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.CreateMaritalStatus;

public class CreateMaritalStatusCommandValidator
    : AbstractValidator<CreateMaritalStatusCommand>
{
    public CreateMaritalStatusCommandValidator()
    {
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
