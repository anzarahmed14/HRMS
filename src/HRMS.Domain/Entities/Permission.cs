using HRMS.Shared.Entities;

namespace HRMS.Domain.Entities;

public class Permission : AuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<RolePermission> RolePermissions { get; set; }
        = new List<RolePermission>();
}