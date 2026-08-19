using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class LeavePolicyRule : AuditableEntity<Guid>
{
    public Guid LeavePolicyId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public decimal AnnualEntitlement { get; set; }
}