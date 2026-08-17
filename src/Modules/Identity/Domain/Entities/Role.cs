using HRMS.BuildingBlocks.Domain.Entities;
namespace HRMS.Modules.Identity.Domain.Entities;

public class Role : AuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();

     public ICollection<RolePermission> RolePermissions { get; set; }
        = new List<RolePermission>();
}