using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand : IRequest
{
    public Guid Id { get; init; }

    public string EmployeeCode { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? PhoneNumber { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public DateOnly DateOfJoining { get; init; }

    public Guid DepartmentId { get; init; }
}