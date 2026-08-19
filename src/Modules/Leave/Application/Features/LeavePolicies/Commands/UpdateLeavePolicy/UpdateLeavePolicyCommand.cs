using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.UpdateLeavePolicy;

public record UpdateLeavePolicyCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid CompanyId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }
}
