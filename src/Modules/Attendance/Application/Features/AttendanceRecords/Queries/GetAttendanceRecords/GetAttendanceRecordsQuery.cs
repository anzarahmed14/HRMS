using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecords;

public sealed record GetAttendanceRecordsQuery(
    PagedRequest Page,
    Guid? EmployeeId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? Status
) : IRequest<PagedResult<AttendanceRecordDto>>;
