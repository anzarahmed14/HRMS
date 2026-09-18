using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Queries.GetAttendanceRegularizationStatusById;

public sealed record GetAttendanceRegularizationStatusByIdQuery(
    Guid Id
) : IRequest<AttendanceRegularizationStatusDto>;
