namespace HRMS.Modules.Identity.Application.Features.Identity.DTOs;

public class PermissionDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
