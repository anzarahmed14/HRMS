using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.DeleteAttendancePolicy;

public sealed record DeleteAttendancePolicyCommand(
    Guid Id
) : IRequest;
