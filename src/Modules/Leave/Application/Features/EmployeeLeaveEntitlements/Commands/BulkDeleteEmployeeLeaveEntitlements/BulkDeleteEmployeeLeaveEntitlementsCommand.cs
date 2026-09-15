using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.BulkDeleteEmployeeLeaveEntitlements;

public record BulkDeleteEmployeeLeaveEntitlementsCommand
    : IRequest<BulkDeleteEmployeeLeaveEntitlementsResult>
{
    public Guid EmployeeId { get; init; }

    public Guid LeaveYearId { get; init; }

    public List<Guid> LeaveTypeIds { get; init; } = [];
}

public record BulkDeleteEmployeeLeaveEntitlementsResult
{
    public int DeletedCount { get; init; }
}
