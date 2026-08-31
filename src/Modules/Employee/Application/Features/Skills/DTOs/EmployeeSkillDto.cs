namespace HRMS.Application.Features.Skills.DTOs;

public class EmployeeSkillDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid SkillId { get; set; }

    public string ProficiencyLevel { get; set; } = string.Empty;

    public decimal YearsOfExperience { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; }
}
