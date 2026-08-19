using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.CreateLeavePolicyRule;

public record CreateLeavePolicyRuleCommand : IRequest<Guid>
{
    public Guid LeavePolicyId { get; init; }

    public Guid LeaveTypeId { get; init; }

    public decimal AnnualEntitlement { get; init; }
}