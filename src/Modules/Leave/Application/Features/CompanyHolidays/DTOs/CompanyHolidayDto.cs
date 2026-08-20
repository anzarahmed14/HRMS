namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.DTOs;

public sealed class CompanyHolidayDto
{
    public Guid Id { get; set; }

    public Guid LeaveYearId { get; set; }

    public DateOnly HolidayDate { get; set; }

    public string Name { get; set; } = string.Empty;

    public string HolidayType { get; set; } = string.Empty;

    public bool IsOptional { get; set; }

    public bool IsActive { get; set; }
}
