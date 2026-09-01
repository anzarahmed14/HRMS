using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Identity.Domain.Entities;

public class UserRole : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid RoleId { get; set; }

    public Role Role { get; set; } = null!;
}