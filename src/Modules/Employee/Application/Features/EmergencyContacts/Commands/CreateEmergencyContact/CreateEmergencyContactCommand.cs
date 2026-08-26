using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Commands.CreateEmergencyContact;

public record CreateEmergencyContactCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid RelationshipId { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string? AlternatePhoneNumber { get; init; }

    public string? Email { get; init; }

    public bool IsPrimary { get; init; }
}
