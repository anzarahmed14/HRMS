using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Queries.GetAttendanceDayStatusById;

public sealed record GetAttendanceDayStatusByIdQuery(
    Guid Id
) : IRequest<AttendanceDayStatusDto>;
