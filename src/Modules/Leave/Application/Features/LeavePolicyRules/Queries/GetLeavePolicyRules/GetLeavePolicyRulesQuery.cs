using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Queries.GetLeavePolicyRules;

public record GetLeavePolicyRulesQuery(
    PagedRequest Request) : IRequest<PagedResult<LeavePolicyRuleListDto>>;

public record LeavePolicyRuleListDto
{
    public Guid Id { get; init; }
    public Guid LeavePolicyId { get; init; }
    public Guid LeaveTypeId { get; init; }
    public decimal AnnualEntitlement { get; init; }
}
