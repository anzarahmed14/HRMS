using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Queries.GetLeavePolicyById;

public record GetLeavePolicyByIdQuery(Guid Id)
    : IRequest<LeavePolicyDto>;

public record LeavePolicyDto
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
