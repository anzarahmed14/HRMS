namespace HRMS.Modules.Identity.Application.Features.Identity.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
