namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveBalance;

public sealed class EmployeeLeaveBalanceDto
{
    public Guid EmployeeId { get; init; }

    public Guid LeaveYearId { get; init; }

    public Guid LeaveTypeId { get; init; }

    public decimal EntitledDays { get; init; }

    public decimal UsedDays { get; init; }

    public decimal AvailableDays { get; init; }
}
