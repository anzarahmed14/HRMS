namespace HRMS.Application.Features.Experiences.DTOs;

public class EmployeeExperienceDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string EmploymentType { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Location { get; set; }

    public string? Responsibilities { get; set; }
}
