using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.CreateEmployeeCertification;

public record CreateEmployeeCertificationCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public Guid CertificationId { get; init; }

    public string? CertificationNumber { get; init; }

    public DateOnly IssueDate { get; init; }

    public DateOnly? ExpiryDate { get; init; }

    public string? CredentialUrl { get; init; }
}
