using FluentValidation;

namespace HRMS.Application.Features.Certifications.Commands.CreateEmployeeCertification;

public class CreateEmployeeCertificationCommandValidator
    : AbstractValidator<CreateEmployeeCertificationCommand>
{
    public CreateEmployeeCertificationCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.CertificationId)
            .NotEmpty();

        RuleFor(x => x.CertificationNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.CertificationNumber));

        RuleFor(x => x.IssueDate)
            .LessThanOrEqualTo(
                DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.IssueDate)
            .When(x => x.ExpiryDate.HasValue);

        RuleFor(x => x.CredentialUrl)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.CredentialUrl));
    }
}
