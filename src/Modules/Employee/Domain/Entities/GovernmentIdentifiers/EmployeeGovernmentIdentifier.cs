using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeGovernmentIdentifier : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid IdentifierTypeId { get; set; }

    public string IdentifierNumber { get; set; } = string.Empty;

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public bool IsVerified { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }
}
