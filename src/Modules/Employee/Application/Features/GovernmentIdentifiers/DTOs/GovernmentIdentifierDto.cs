namespace HRMS.Application.Features.GovernmentIdentifiers.DTOs;

public class GovernmentIdentifierDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid IdentifierTypeId { get; set; }

    public string MaskedIdentifierNumber { get; set; } = string.Empty;

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public bool IsVerified { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }
}
