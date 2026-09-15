using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.DeleteLeaveRequestStatus;

public record DeleteLeaveRequestStatusCommand(
    Guid Id) : IRequest;
