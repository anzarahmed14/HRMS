using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.DeleteAttendanceRegularizationStatus;

public sealed record DeleteAttendanceRegularizationStatusCommand(
    Guid Id
) : IRequest;
