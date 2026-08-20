using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveEntitlements;

public class GetEmployeeLeaveEntitlementsQueryHandler
    : IRequestHandler<
        GetEmployeeLeaveEntitlementsQuery,
        PagedResult<EmployeeLeaveEntitlementListDto>>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _repository;

    public GetEmployeeLeaveEntitlementsQueryHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeLeaveEntitlementListDto>> Handle(
        GetEmployeeLeaveEntitlementsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeLeaveEntitlementListDto>
        {
            Items = result.Items
                .Select(x => new EmployeeLeaveEntitlementListDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    LeaveYearId = x.LeaveYearId,
                    LeaveTypeId = x.LeaveTypeId,
                    LeavePolicyRuleId = x.LeavePolicyRuleId,
                    EntitledDays = x.EntitledDays
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
