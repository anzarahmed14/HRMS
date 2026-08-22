using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Queries.GetEmployeeShiftAssignments;

public sealed class GetEmployeeShiftAssignmentsQueryHandler
    : IRequestHandler<
        GetEmployeeShiftAssignmentsQuery,
        PagedResult<EmployeeShiftAssignmentDto>>
{
    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _repository;

    public GetEmployeeShiftAssignmentsQueryHandler(
        IReadRepository<EmployeeShiftAssignment, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeShiftAssignmentDto>> Handle(
        GetEmployeeShiftAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeShiftAssignmentDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeShiftAssignmentDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    AttendanceShiftId = x.AttendanceShiftId,
                    AttendancePolicyId = x.AttendancePolicyId,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo,
                    IsPrimary = x.IsPrimary,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
