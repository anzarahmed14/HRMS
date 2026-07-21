using HRMS.Shared.Interfaces;

namespace HRMS.API.Services;

public class CurrentUserContext : IUserContext
{
    public Guid? UserId => Guid.Empty;

    public string? UserName => "System";

    public bool IsAuthenticated => false;
}