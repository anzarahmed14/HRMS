using HRMS.Shared.Entities;

namespace HRMS.Domain.Entities;

public class Department : AuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}