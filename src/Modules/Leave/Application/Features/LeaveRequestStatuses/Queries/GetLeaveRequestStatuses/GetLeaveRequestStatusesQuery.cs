using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatuses;

public record GetLeaveRequestStatusesQuery(
    PagedRequest Request) : IRequest<PagedResult<LeaveRequestStatusDto>>;
