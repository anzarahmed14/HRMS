using FluentValidation;

namespace HRMS.Application.Features.Certifications.Commands.DeleteEmployeeCertification;

public class DeleteEmployeeCertificationCommandValidator
    : AbstractValidator<DeleteEmployeeCertificationCommand>
{
    public DeleteEmployeeCertificationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
