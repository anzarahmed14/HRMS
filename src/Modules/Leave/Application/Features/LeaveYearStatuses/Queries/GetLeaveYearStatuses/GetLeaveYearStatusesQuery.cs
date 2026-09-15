using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatuses;

public record GetLeaveYearStatusesQuery(
    PagedRequest Request) : IRequest<PagedResult<LeaveYearStatusListDto>>;

public record LeaveYearStatusListDto
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
