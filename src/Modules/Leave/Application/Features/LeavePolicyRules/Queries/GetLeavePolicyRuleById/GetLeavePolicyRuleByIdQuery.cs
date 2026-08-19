using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Queries.GetLeavePolicyRuleById;

public record GetLeavePolicyRuleByIdQuery(Guid Id)
    : IRequest<LeavePolicyRuleDto>;

public record LeavePolicyRuleDto
{
    public Guid Id { get; init; }
    public Guid LeavePolicyId { get; init; }
    public Guid LeaveTypeId { get; init; }
    public decimal AnnualEntitlement { get; init; }
}
