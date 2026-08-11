namespace HRMS.Shared.Interfaces;

public interface IUserContext
{
    Guid? UserId { get; }

    Guid? EmployeeId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}