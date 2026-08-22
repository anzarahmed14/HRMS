using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Queries.GetAttendanceShiftById;

public sealed record GetAttendanceShiftByIdQuery(
    Guid Id
) : IRequest<AttendanceShiftDto>;
