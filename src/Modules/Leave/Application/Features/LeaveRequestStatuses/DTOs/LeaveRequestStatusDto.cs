namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.DTOs;

public record LeaveRequestStatusDto
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
