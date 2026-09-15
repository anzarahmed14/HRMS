using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Commands.CreateGender;

public class CreateGenderCommandValidator
    : AbstractValidator<CreateGenderCommand>
{
    public CreateGenderCommandValidator()
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
