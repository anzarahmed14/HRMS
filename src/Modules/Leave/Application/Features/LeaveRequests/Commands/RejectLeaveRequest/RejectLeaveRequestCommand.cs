using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.RejectLeaveRequest;

public sealed record RejectLeaveRequestCommand(
    Guid Id,
    string RejectionReason
) : IRequest;
