using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecordById;

public sealed record GetAttendanceRecordByIdQuery(
    Guid Id
) : IRequest<AttendanceRecordDto>;
