using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class LeaveYear : AuditableEntity<Guid>
{
    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid StatusId { get; set; }
}