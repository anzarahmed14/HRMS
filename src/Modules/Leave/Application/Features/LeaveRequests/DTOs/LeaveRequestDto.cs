namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;

public class LeaveRequestDto
{
    public Guid Id { get; set; }

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
}