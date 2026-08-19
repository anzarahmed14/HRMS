namespace HRMS.Modules.Leave.Application.Features.LeaveYears.DTOs;

public class LeaveYearDto
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid StatusId { get; set; }
}