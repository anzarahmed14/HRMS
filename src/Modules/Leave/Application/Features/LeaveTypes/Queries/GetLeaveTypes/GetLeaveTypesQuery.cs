using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Queries.GetLeaveTypes;

public record GetLeaveTypesQuery(
    PagedRequest Request) : IRequest<PagedResult<LeaveTypeListDto>>;

public record LeaveTypeListDto
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsPaid { get; init; }
    public bool IsActive { get; init; }
}
