using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class LeavePolicy : AuditableEntity<Guid>
{
    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}