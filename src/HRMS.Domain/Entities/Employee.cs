using HRMS.Shared.Entities;

namespace HRMS.Domain.Entities;

public class Employee : AuditableEntity<Guid>
{
    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly DateOfJoining { get; set; }

    // Foreign Key
    public Guid  DepartmentId { get; set; }

    // Navigation Property
    public Department? Department { get; set; }
}