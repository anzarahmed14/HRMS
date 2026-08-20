using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;

public sealed record DeleteLeaveRequestCommand(
    Guid Id
) : IRequest;
