using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Queries.GetLeavePolicies;

public record GetLeavePoliciesQuery(
    PagedRequest Request) : IRequest<PagedResult<LeavePolicyListDto>>;

public record LeavePolicyListDto
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
