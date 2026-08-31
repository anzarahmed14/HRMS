using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeLanguage : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid LanguageId { get; set; }

    public string ProficiencyLevel { get; set; } = string.Empty;

    public bool CanRead { get; set; }

    public bool CanWrite { get; set; }

    public bool CanSpeak { get; set; }

    public bool IsActive { get; set; } = true;
}
