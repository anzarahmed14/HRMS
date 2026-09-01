using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Identity.Domain.Entities;

public class RolePermission : AuditableEntity<Guid>
{
    public Guid RoleId { get; set; }

    public Role Role { get; set; } = null!;

    public Guid PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}