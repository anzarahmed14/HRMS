using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeCertification : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid CertificationId { get; set; }

    public string? CertificationNumber { get; set; }

    public DateOnly IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? CredentialUrl { get; set; }

    public bool IsVerified { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }

    public bool IsActive { get; set; } = true;
}
