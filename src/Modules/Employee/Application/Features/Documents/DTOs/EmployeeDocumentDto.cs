namespace HRMS.Application.Features.Documents.DTOs;

public class EmployeeDocumentDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid DocumentTypeId { get; set; }

    public string DocumentName { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTimeOffset UploadedOn { get; set; }

    public bool IsVerified { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }

    public bool IsActive { get; set; }
}
