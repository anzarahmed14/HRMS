using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;

public sealed record CreateLeaveRequestCommand(
    Guid EmployeeId,
    Guid LeaveYearId,
    Guid LeaveTypeId,
    Guid StartDayPartId,
    Guid EndDayPartId,
    DateOnly FromDate,
    DateOnly ToDate,
    string Reason
) : IRequest<Guid>;
