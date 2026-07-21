namespace HRMS.Shared.Interfaces;

public interface IUserContext
{
    Guid? UserId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}