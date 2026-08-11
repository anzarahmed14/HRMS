using System.Security.Claims;
using HRMS.Shared.Interfaces;

namespace HRMS.API.Services;

public class CurrentUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("userId")?
                .Value;

            return Guid.TryParse(userId, out var id)
                ? id
                : null;
        }
    }

    public Guid? EmployeeId
    {
        get
        {
            var employeeId = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("employeeId")?
                .Value;

            return Guid.TryParse(employeeId, out var id)
                ? id
                : null;
        }
    }

    public string? UserName
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Name)?
                .Value;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                .User
                .Identity?
                .IsAuthenticated ?? false;
        }
    }
}