namespace HRMS.Application.Features.Certifications.DTOs;

public class EmployeeCertificationDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid CertificationId { get; set; }

    public string? CertificationNumber { get; set; }

    public DateOnly IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? CredentialUrl { get; set; }

    public bool IsVerified { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }

    public bool IsActive { get; set; }
}
