using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;

public sealed record UpdateLeaveRequestCommand(
    Guid Id,
    Guid LeaveTypeId,
    Guid StartDayPartId,
    Guid EndDayPartId,
    DateOnly FromDate,
    DateOnly ToDate,
    string Reason
) : IRequest;
