namespace HRMS.Application.Features.Employees.DTOs;

public class UpdateEmployeeDto
{
    public Guid Id { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly DateOfJoining { get; set; }

    public Guid DepartmentId { get; set; }
}