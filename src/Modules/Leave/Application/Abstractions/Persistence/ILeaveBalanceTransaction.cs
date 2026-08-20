namespace HRMS.Modules.Leave.Application.Abstractions.Persistence;

public interface ILeaveBalanceTransaction
{
    Task ApproveLeaveAsync(
        Guid leaveRequestId,
        decimal days,
        Guid approvedStatusId,
        DateTimeOffset approvedOn,
        string? approvalReason,
        CancellationToken cancellationToken = default);

    Task CancelLeaveAsync(
        Guid leaveRequestId,
        decimal days,
        Guid cancelledStatusId,
        DateTimeOffset cancelledOn,
        string cancellationReason,
        CancellationToken cancellationToken = default);
}
