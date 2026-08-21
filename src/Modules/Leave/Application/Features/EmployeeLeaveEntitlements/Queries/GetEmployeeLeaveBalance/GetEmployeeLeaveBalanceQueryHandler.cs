using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveBalance;

public sealed class GetEmployeeLeaveBalanceQueryHandler
    : IRequestHandler<
        GetEmployeeLeaveBalanceQuery,
        IReadOnlyList<EmployeeLeaveBalanceDto>>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _repository;

    public GetEmployeeLeaveBalanceQueryHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<EmployeeLeaveBalanceDto>> Handle(
        GetEmployeeLeaveBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var entitlements = await _repository.FindAsync(
            x =>
                x.EmployeeId == request.EmployeeId &&
                x.LeaveYearId == request.LeaveYearId &&
                !x.IsDeleted &&
                (!request.LeaveTypeId.HasValue ||
                 x.LeaveTypeId == request.LeaveTypeId.Value),
            cancellationToken);

        return entitlements
            .Select(x => new EmployeeLeaveBalanceDto
            {
                EmployeeId = x.EmployeeId,
                LeaveYearId = x.LeaveYearId,
                LeaveTypeId = x.LeaveTypeId,
                EntitledDays = x.EntitledDays,
                UsedDays = x.UsedDays,
                AvailableDays =
                    x.EntitledDays
                    + x.CarryForwardDays
                    - x.UsedDays
            })
            .ToList();
    }
}