using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.DeleteLeavePolicyRule;

public record DeleteLeavePolicyRuleCommand(
    Guid Id) : IRequest;
