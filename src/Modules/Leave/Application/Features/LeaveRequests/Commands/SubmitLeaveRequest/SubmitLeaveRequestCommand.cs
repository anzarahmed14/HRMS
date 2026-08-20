using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.SubmitLeaveRequest;

public sealed record SubmitLeaveRequestCommand(
    Guid Id
) : IRequest;
