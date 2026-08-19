using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class LeaveYearStatus : AuditableEntity<Guid>
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}