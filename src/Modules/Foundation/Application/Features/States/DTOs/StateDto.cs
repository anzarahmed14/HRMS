namespace HRMS.Modules.Foundation.Application.Features.States.DTOs;

public sealed class StateDto
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
