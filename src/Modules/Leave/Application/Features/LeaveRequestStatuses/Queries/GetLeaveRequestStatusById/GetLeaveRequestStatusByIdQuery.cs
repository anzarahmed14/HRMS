using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatusById;

public record GetLeaveRequestStatusByIdQuery(Guid Id)
    : IRequest<LeaveRequestStatusDto>;
