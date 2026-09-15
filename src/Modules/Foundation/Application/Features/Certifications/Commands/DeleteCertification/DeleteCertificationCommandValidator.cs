using FluentValidation;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.DeleteCertification;

public class DeleteCertificationCommandValidator
    : AbstractValidator<DeleteCertificationCommand>
{
    public DeleteCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
