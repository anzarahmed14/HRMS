using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.CreateLeavePolicy;

public record CreateLeavePolicyCommand : IRequest<Guid>
{
    public Guid CompanyId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}