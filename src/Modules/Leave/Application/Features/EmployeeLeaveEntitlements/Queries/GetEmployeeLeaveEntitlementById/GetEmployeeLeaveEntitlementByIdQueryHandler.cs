using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveEntitlementById;

public class GetEmployeeLeaveEntitlementByIdQueryHandler
    : IRequestHandler<GetEmployeeLeaveEntitlementByIdQuery, EmployeeLeaveEntitlementDto>
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid> _repository;

    public GetEmployeeLeaveEntitlementByIdQueryHandler(
        IReadRepository<EmployeeLeaveEntitlement, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeLeaveEntitlementDto> Handle(
        GetEmployeeLeaveEntitlementByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Employee Leave Entitlement",
                request.Id);
        }

        return new EmployeeLeaveEntitlementDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            LeaveYearId = entity.LeaveYearId,
            LeaveTypeId = entity.LeaveTypeId,
            LeavePolicyRuleId = entity.LeavePolicyRuleId,
            EntitledDays = entity.EntitledDays
        };
    }
}
