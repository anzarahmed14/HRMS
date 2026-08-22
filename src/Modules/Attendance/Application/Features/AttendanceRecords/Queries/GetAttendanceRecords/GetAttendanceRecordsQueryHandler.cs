using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecords;

public sealed class GetAttendanceRecordsQueryHandler
    : IRequestHandler<
        GetAttendanceRecordsQuery,
        PagedResult<AttendanceRecordDto>>
{
    private readonly IReadRepository<AttendanceRecord, Guid>
        _repository;

    public GetAttendanceRecordsQueryHandler(
        IReadRepository<AttendanceRecord, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AttendanceRecordDto>> Handle(
        GetAttendanceRecordsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Page,
            x =>
                !x.IsDeleted &&
                (!request.EmployeeId.HasValue ||
                 x.EmployeeId == request.EmployeeId.Value) &&
                (!request.FromDate.HasValue ||
                 x.AttendanceDate >= request.FromDate.Value) &&
                (!request.ToDate.HasValue ||
                 x.AttendanceDate <= request.ToDate.Value) &&
                (string.IsNullOrWhiteSpace(request.Status) ||
                 x.Status == request.Status),
            query => query.OrderByDescending(
                x => x.AttendanceDate),
            cancellationToken);

        return new PagedResult<AttendanceRecordDto>
        {
            Items = result.Items
        .Select(x => new AttendanceRecordDto
        {
            Id = x.Id,
            EmployeeId = x.EmployeeId,
            AttendanceShiftId = x.AttendanceShiftId,
            AttendancePolicyId = x.AttendancePolicyId,
            AttendanceDate = x.AttendanceDate,
            CheckIn = x.CheckIn,
            CheckOut = x.CheckOut,
            WorkedMinutes = x.WorkedMinutes,
            LateMinutes = x.LateMinutes,
            EarlyLeaveMinutes = x.EarlyLeaveMinutes,
            OvertimeMinutes = x.OvertimeMinutes,
            Status = x.Status,
            Remarks = x.Remarks
        })
        .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
