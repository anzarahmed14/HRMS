namespace HRMS.Modules.Foundation.Application.Features.Certifications.DTOs;

public record CertificationDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? IssuingOrganization { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
