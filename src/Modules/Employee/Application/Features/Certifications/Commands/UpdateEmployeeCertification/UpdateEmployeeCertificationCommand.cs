using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.UpdateEmployeeCertification;

public record UpdateEmployeeCertificationCommand : IRequest
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public Guid CertificationId { get; init; }
    public string? CertificationNumber { get; init; }
    public DateOnly IssueDate { get; init; }
    public DateOnly? ExpiryDate { get; init; }
    public string? CredentialUrl { get; init; }
    public bool IsActive { get; init; } = true;
}
