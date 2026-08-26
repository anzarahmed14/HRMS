namespace HRMS.Application.Features.EmergencyContacts.DTOs;

public class EmergencyContactDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid RelationshipId { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? AlternatePhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool IsPrimary { get; set; }
}
