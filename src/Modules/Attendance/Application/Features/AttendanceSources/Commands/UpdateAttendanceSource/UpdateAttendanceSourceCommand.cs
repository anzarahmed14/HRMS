using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.UpdateAttendanceSource;

public sealed record UpdateAttendanceSourceCommand(
    Guid Id,
    string Code,
    string Name,
    string SourceType,
    string? Description,
    bool IsActive
) : IRequest;
