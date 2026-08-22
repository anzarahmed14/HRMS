using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.RejectAttendanceRegularization;

public sealed record RejectAttendanceRegularizationCommand(
    Guid Id,
    string? Remarks
) : IRequest;
