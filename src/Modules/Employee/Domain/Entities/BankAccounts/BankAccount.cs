using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class BankAccount : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public string AccountHolderName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string BankName { get; set; } = string.Empty;

    public string IFSCCode { get; set; } = string.Empty;

    public string? BranchName { get; set; }

    public string AccountType { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
}
