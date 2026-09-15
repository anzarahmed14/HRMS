namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.DTOs;

public record IdentifierTypeDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsSensitive { get; init; }
    public bool IsActive { get; init; }
}
