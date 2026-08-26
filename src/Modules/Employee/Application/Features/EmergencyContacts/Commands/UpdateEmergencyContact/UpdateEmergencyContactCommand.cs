using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Commands.UpdateEmergencyContact;

public record UpdateEmergencyContactCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid RelationshipId { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string? AlternatePhoneNumber { get; init; }

    public string? Email { get; init; }

    public bool IsPrimary { get; init; }
}
