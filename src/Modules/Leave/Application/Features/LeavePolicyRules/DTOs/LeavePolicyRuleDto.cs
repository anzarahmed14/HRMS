namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.DTOs;

public class LeavePolicyRuleDto
{
    public Guid Id { get; set; }

    public Guid LeavePolicyId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public decimal AnnualEntitlement { get; set; }
}