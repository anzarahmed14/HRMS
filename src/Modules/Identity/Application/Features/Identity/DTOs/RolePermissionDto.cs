namespace HRMS.Modules.Identity.Application.Features.Identity.DTOs;

public class RolePermissionDto
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }

    public string PermissionName { get; set; } = string.Empty;

    public string? PermissionDescription { get; set; }
}
