using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Department.Domain.Entities;

public class Department : AuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}