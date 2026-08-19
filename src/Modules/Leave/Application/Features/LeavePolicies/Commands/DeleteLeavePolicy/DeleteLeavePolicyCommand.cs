using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.DeleteLeavePolicy;

public record DeleteLeavePolicyCommand(
    Guid Id) : IRequest;
