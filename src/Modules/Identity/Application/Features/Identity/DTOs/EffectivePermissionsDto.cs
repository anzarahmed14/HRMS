namespace HRMS.Modules.Identity.Application.Features.Identity.DTOs;

public class EffectivePermissionsDto
{
    public IReadOnlyList<string> Roles { get; set; } = new List<string>();

    public IReadOnlyList<string> Permissions { get; set; } = new List<string>();
}
