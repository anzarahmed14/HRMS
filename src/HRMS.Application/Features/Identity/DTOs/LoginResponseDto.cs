namespace HRMS.Application.Features.Identity.DTOs;

public class LoginResponseDto
{
    public Guid UserId { get; set; }

    public Guid EmployeeId { get; set; }

    public string UserName { get; set; } = string.Empty;
}