namespace HRMS.Application.Features.EmployeeContacts.DTOs;

public class EmployeeContactDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string ContactType { get; set; } = string.Empty;

    public string ContactValue { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
}
