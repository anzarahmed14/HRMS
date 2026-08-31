namespace HRMS.Application.Features.Dependents.DTOs;

public class EmployeeDependentDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid RelationshipId { get; set; }

    public Guid GenderId { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool IsDependent { get; set; }

    public bool IsActive { get; set; }
}
