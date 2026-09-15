using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.CreateLeaveYearStatus;

public record CreateLeaveYearStatusCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
