namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.DTOs;

public class EmployeeLeaveEntitlementDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid LeaveYearId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public Guid LeavePolicyRuleId { get; set; }

    public decimal EntitledDays { get; set; }
}