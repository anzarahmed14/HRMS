using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Queries.GetAttendanceShifts;

public sealed class GetAttendanceShiftsQueryHandler
    : IRequestHandler<
        GetAttendanceShiftsQuery,
        PagedResult<AttendanceShiftDto>>
{
    private readonly IReadRepository<AttendanceShift, Guid> _repository;

    public GetAttendanceShiftsQueryHandler(
        IReadRepository<AttendanceShift, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendanceShiftDto>> Handle(
        GetAttendanceShiftsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AttendanceShiftDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AttendanceShiftDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    BreakMinutes = x.BreakMinutes,
                    IsOvernight = x.IsOvernight,
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
