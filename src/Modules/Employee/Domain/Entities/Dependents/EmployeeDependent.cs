using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeDependent : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid RelationshipId { get; set; }

    public Guid GenderId { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool IsDependent { get; set; } = true;

    public bool IsActive { get; set; } = true;
}
