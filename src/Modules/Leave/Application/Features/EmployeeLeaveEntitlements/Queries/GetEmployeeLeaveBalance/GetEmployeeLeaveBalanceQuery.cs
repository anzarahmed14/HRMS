using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveBalance;

public sealed record GetEmployeeLeaveBalanceQuery(
    Guid EmployeeId,
    Guid LeaveYearId,
    Guid? LeaveTypeId)
    : IRequest<IReadOnlyList<EmployeeLeaveBalanceDto>>;
