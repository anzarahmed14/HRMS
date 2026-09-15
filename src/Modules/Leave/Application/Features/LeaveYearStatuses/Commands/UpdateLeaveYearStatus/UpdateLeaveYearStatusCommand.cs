using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.UpdateLeaveYearStatus;

public record UpdateLeaveYearStatusCommand : IRequest
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
