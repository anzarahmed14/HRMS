namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.DTOs;

public record MaritalStatusDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
