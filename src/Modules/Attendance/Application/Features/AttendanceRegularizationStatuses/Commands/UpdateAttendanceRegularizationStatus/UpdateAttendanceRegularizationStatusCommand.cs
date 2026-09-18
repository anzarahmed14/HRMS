using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.UpdateAttendanceRegularizationStatus;

public sealed record UpdateAttendanceRegularizationStatusCommand(
    Guid Id,
    string Code,
    string Name,
    bool IsActive
) : IRequest;
