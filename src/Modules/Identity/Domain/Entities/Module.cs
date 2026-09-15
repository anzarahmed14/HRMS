using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Identity.Domain.Entities;

public class Module : AuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Permission> Permissions { get; set; }
        = new List<Permission>();
}
