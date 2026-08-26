using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Foundation.Domain.Entities;

public class State : AuditableEntity<Guid>
{
    public Guid CountryId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
