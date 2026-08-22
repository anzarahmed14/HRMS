using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizationById;

public sealed record GetAttendanceRegularizationByIdQuery(
    Guid Id
) : IRequest<AttendanceRegularizationDto>;
