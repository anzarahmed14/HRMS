using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.UpdateLeavePolicyRule;

public record UpdateLeavePolicyRuleCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid LeavePolicyId { get; init; }

    public Guid LeaveTypeId { get; init; }

    public decimal AnnualEntitlement { get; init; }
}
