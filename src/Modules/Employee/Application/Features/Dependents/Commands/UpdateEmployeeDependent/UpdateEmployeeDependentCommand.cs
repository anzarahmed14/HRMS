using MediatR;

namespace HRMS.Application.Features.Dependents.Commands.UpdateEmployeeDependent;

public record UpdateEmployeeDependentCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid RelationshipId { get; init; }

    public Guid GenderId { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Email { get; init; }

    public bool IsDependent { get; init; } = true;

    public bool IsActive { get; init; } = true;
}
