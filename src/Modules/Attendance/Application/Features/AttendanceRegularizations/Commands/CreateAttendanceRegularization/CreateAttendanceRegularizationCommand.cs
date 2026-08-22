using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.CreateAttendanceRegularization;

public sealed record CreateAttendanceRegularizationCommand(
    Guid EmployeeId,
    DateOnly AttendanceDate,
    DateTimeOffset? RequestedCheckIn,
    DateTimeOffset? RequestedCheckOut,
    string Reason
) : IRequest<Guid>;
