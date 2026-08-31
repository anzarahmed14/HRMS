using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeSkill : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid SkillId { get; set; }

    public string ProficiencyLevel { get; set; } = string.Empty;

    public decimal YearsOfExperience { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; } = true;
}
