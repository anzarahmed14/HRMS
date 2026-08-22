using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.UpdateEmployeeShiftAssignment;

public sealed record UpdateEmployeeShiftAssignmentCommand(
    Guid Id,
    Guid AttendanceShiftId,
    Guid AttendancePolicyId,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsPrimary,
    bool IsActive
) : IRequest;
