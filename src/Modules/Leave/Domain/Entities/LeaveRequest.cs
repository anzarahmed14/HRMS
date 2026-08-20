using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class LeaveRequest : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveYearId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public Guid StartDayPartId { get; set; }

    public Guid EndDayPartId { get; set; }

    public Guid StatusId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public decimal TotalDays { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTimeOffset AppliedOn { get; set; }

    // Approval
    public Guid? ApprovedBy { get; set; }

    public DateTimeOffset? ApprovedOn { get; set; }

    public string? ApprovalReason { get; set; }
    // Reject
    public Guid? RejectedBy { get; set; }

    public DateTimeOffset? RejectedOn { get; set; }

    public string? RejectionReason { get; set; }

    //Cancel
    public Guid? CancelledBy { get; set; }

    public DateTimeOffset? CancelledOn { get; set; }

    public string? CancellationReason { get; set; }
}
