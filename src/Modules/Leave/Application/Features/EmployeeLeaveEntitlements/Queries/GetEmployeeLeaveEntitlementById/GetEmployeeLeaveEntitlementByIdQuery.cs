using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveEntitlementById;

public record GetEmployeeLeaveEntitlementByIdQuery(Guid Id)
    : IRequest<EmployeeLeaveEntitlementDto>;

public record EmployeeLeaveEntitlementDto
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public Guid LeaveYearId { get; init; }
    public Guid LeaveTypeId { get; init; }
    public Guid LeavePolicyRuleId { get; init; }
    public decimal EntitledDays { get; init; }
}
