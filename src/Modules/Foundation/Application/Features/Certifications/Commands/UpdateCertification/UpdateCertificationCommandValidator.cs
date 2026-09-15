using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.UpdateCertification;

public class UpdateCertificationCommandValidator
    : AbstractValidator<UpdateCertificationCommand>
{
    public UpdateCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.IssuingOrganization)
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
