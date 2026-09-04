namespace HRMS.Modules.Companies.Application.Features.Companies.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
