using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.CreateAttendanceRegularizationStatus;

public sealed record CreateAttendanceRegularizationStatusCommand(
    string Code,
    string Name,
    bool IsActive
) : IRequest<Guid>;
