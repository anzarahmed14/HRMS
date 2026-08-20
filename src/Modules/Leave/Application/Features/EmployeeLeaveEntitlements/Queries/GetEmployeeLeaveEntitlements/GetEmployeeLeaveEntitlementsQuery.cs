using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveEntitlements;

public record GetEmployeeLeaveEntitlementsQuery(
    PagedRequest Request) : IRequest<PagedResult<EmployeeLeaveEntitlementListDto>>;

public record EmployeeLeaveEntitlementListDto
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public Guid LeaveYearId { get; init; }
    public Guid LeaveTypeId { get; init; }
    public Guid LeavePolicyRuleId { get; init; }
    public decimal EntitledDays { get; init; }
}
