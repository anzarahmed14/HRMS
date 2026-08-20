using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;

public sealed record CancelLeaveRequestCommand(
    Guid Id,
    string CancellationReason
) : IRequest;
