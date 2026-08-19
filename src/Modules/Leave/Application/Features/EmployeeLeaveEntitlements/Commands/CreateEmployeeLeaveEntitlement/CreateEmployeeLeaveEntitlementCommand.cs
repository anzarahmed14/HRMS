using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.CreateEmployeeLeaveEntitlement;

public record CreateEmployeeLeaveEntitlementCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public Guid LeaveYearId { get; init; }

    public Guid LeaveTypeId { get; init; }

    public Guid LeavePolicyRuleId { get; init; }

    public decimal EntitledDays { get; init; }
}