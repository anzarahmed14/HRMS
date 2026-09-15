using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Commands.DeleteGender;

public class DeleteGenderCommandValidator
    : AbstractValidator<DeleteGenderCommand>
{
    public DeleteGenderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
