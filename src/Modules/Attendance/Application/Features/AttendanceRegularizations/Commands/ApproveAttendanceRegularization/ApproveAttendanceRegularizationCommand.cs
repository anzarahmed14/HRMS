using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.ApproveAttendanceRegularization;

public sealed record ApproveAttendanceRegularizationCommand(
    Guid Id,
    string? Remarks
) : IRequest;
