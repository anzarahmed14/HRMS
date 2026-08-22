using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.DeleteAttendanceRegularization;

public sealed record DeleteAttendanceRegularizationCommand(
    Guid Id
) : IRequest;
