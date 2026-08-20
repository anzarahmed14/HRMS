using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Leave.Domain.Entities;

public class CompanyHoliday : AuditableEntity<Guid>
{
    public Guid LeaveYearId { get; set; }

    public DateOnly HolidayDate { get; set; }

    public string Name { get; set; } = string.Empty;

    public string HolidayType { get; set; } = "COMPANY";

    public bool IsOptional { get; set; }

    public bool IsActive { get; set; } = true;

    public LeaveYear LeaveYear { get; set; } = null!;
}
