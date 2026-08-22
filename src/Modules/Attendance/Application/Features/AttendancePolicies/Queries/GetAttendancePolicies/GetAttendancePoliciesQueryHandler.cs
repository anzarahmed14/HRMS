using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Queries.GetAttendancePolicies;

public sealed class GetAttendancePoliciesQueryHandler
    : IRequestHandler<
        GetAttendancePoliciesQuery,
        PagedResult<AttendancePolicyDto>>
{
    private readonly IReadRepository<AttendancePolicy, Guid> _repository;

    public GetAttendancePoliciesQueryHandler(
        IReadRepository<AttendancePolicy, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendancePolicyDto>> Handle(
        GetAttendancePoliciesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AttendancePolicyDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AttendancePolicyDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    GracePeriodMinutes = x.GracePeriodMinutes,
                    MinimumWorkingMinutes = x.MinimumWorkingMinutes,
                    FullDayMinutes = x.FullDayMinutes,
                    HalfDayMinutes = x.HalfDayMinutes,
                    IsOvertimeAllowed = x.IsOvertimeAllowed,
                    MinimumOvertimeMinutes = x.MinimumOvertimeMinutes,
                    MaximumOvertimeMinutes = x.MaximumOvertimeMinutes,
                    OvertimeRequiresApproval = x.OvertimeRequiresApproval,
                    IsDefault = x.IsDefault,
                    IsActive = x.IsActive,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
