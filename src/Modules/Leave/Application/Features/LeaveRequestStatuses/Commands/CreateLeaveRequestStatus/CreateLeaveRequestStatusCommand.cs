using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.CreateLeaveRequestStatus;

public record CreateLeaveRequestStatusCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
