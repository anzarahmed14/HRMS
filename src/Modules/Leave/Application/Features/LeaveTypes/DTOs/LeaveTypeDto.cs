namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.DTOs;

public class LeaveTypeDto
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsPaid { get; set; }

    public bool IsActive { get; set; }
}