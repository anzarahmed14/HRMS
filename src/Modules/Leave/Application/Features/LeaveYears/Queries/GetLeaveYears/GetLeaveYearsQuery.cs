using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Queries.GetLeaveYears;

public record GetLeaveYearsQuery(
    PagedRequest Request) : IRequest<PagedResult<LeaveYearListDto>>;

public record LeaveYearListDto
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public Guid StatusId { get; init; }
}
