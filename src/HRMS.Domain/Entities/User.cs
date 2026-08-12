using HRMS.Shared.Entities;

namespace HRMS.Domain.Entities;

public class User : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public string UserName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}