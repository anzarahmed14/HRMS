using HRMS.Modules.Attendance.Application.Features.AttendanceSources.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Queries.GetAttendanceSourceById;

public sealed record GetAttendanceSourceByIdQuery(
    Guid Id
) : IRequest<AttendanceSourceDto>;
