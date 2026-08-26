using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeContact : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public string ContactType { get; set; } = string.Empty;

    public string ContactValue { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
}
