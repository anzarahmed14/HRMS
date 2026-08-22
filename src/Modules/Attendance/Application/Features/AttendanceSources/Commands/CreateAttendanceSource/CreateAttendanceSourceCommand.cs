using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.CreateAttendanceSource;

public sealed record CreateAttendanceSourceCommand(
    Guid CompanyId,
    string Code,
    string Name,
    string SourceType,
    string? Description,
    bool IsActive
) : IRequest<Guid>;
