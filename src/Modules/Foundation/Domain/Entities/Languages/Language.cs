using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Foundation.Domain.Entities;

public class Language : AuditableEntity<Guid>
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
