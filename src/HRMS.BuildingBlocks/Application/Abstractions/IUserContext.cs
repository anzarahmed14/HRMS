namespace HRMS.BuildingBlocks.Application.Abstractions;

public interface IUserContext
{
    Guid? UserId { get; }

    Guid? EmployeeId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}