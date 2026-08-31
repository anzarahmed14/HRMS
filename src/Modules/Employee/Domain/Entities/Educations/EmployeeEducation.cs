using HRMS.BuildingBlocks.Domain.Entities;

namespace HRMS.Modules.Employee.Domain.Entities;

public class EmployeeEducation : AuditableEntity<Guid>
{
    public Guid EmployeeId { get; set; }

    public string EducationLevel { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public string? Specialization { get; set; }

    public string InstitutionName { get; set; } = string.Empty;

    public string? UniversityName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Grade { get; set; }

    public bool IsHighestQualification { get; set; }

    public bool IsVerified { get; set; }

    public DateTimeOffset? VerifiedOn { get; set; }
}
