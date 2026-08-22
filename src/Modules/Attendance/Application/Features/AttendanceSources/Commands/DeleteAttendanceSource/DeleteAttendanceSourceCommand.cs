using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.DeleteAttendanceSource;

public sealed record DeleteAttendanceSourceCommand(
    Guid Id
) : IRequest;
