using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.BulkUpsertEmployeeLeaveEntitlements;

public record BulkUpsertEmployeeLeaveEntitlementsCommand
    : IRequest<BulkUpsertEmployeeLeaveEntitlementsResult>
{
    public Guid EmployeeId { get; init; }

    public Guid LeaveYearId { get; init; }

    public List<BulkEmployeeLeaveEntitlementItem> Entitlements { get; init; } = [];
}

public record BulkEmployeeLeaveEntitlementItem
{
    public Guid LeaveTypeId { get; init; }

    public Guid LeavePolicyRuleId { get; init; }

    public decimal EntitledDays { get; init; }
}

public record BulkUpsertEmployeeLeaveEntitlementsResult
{
    public int CreatedCount { get; init; }

    public int UpdatedCount { get; init; }

    public int TotalCount => CreatedCount + UpdatedCount;
}
