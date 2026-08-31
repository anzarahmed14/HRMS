using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeNominee : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid RelationshipId { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool IsMinor { get; set; }

    public bool IsActive { get; set; } = true;
}
