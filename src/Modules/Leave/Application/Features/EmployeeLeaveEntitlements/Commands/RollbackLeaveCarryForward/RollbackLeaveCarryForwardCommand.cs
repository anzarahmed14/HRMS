using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.RollbackLeaveCarryForward;

public sealed record RollbackLeaveCarryForwardCommand(
    Guid LeaveYearId
) : IRequest;
