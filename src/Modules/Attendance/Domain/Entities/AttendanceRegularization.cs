using HRMS.BuildingBlocks.Domain.Entities;

public class AttendanceRegularization : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid AttendanceRecordId { get; set; }

    public Guid AttendanceRegularizationStatusId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public DateTimeOffset? RequestedCheckIn { get; set; }

    public DateTimeOffset? RequestedCheckOut { get; set; }

    public string Reason { get; set; } = string.Empty;

    public Guid? RequestedBy { get; set; }

    public DateTimeOffset RequestedOn { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTimeOffset? ApprovedOn { get; set; }

    public string? ApprovalRemarks { get; set; }

    public Guid? RejectedBy { get; set; }

    public DateTimeOffset? RejectedOn { get; set; }

    public string? RejectionRemarks { get; set; }

    public Guid? CancelledBy { get; set; }

    public DateTimeOffset? CancelledOn { get; set; }

    public string? CancellationRemarks { get; set; }
}