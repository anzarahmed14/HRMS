using HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;

public sealed record GetLeaveRequestByIdQuery(
    Guid Id
) : IRequest<LeaveRequestDto>;
