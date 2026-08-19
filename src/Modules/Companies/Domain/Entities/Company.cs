using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Companies.Domain.Entities;

public class Company : AuditableEntity<Guid>
{
    public string CompanyCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}