using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Identity.Domain.Entities;

public class User : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}