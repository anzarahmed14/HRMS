using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequests;

public sealed class GetLeaveRequestsQuery : PagedRequest, IRequest<PagedResult<LeaveRequestDto>>
{
}
