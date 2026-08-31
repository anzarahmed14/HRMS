using MediatR;

namespace HRMS.Application.Features.Nominees.Commands.UpdateEmployeeNominee;

public record UpdateEmployeeNomineeCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid RelationshipId { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Email { get; init; }

    public bool IsMinor { get; init; }

    public bool IsActive { get; init; } = true;
}
