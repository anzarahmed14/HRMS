namespace HRMS.Modules.Identity.Application.Features.Identity.DTOs;

public class UserRoleDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public string? RoleDescription { get; set; }
}
