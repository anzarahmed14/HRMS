using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class EmployeeLeaveEntitlement : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveYearId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public Guid LeavePolicyRuleId { get; set; }

    public decimal EntitledDays { get; set; }

    public decimal CarryForwardDays { get; set; }

    public decimal UsedDays { get; set; }
}