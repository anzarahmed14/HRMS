namespace HRMS.Application.Features.Languages.DTOs;

public class EmployeeLanguageDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid LanguageId { get; set; }

    public string ProficiencyLevel { get; set; } = string.Empty;

    public bool CanRead { get; set; }

    public bool CanWrite { get; set; }

    public bool CanSpeak { get; set; }

    public bool IsActive { get; set; }
}
