using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest;

public sealed record ApproveLeaveRequestCommand(
    Guid Id,
    string? ApprovalReason
) : IRequest;
