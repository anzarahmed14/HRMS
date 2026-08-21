using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.GenerateLeaveCarryForward;

public sealed record GenerateLeaveCarryForwardCommand(
    Guid LeaveYearId
) : IRequest;
