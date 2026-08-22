using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.CreateEmployeeShiftAssignment;

public sealed record CreateEmployeeShiftAssignmentCommand(
    Guid EmployeeId,
    Guid AttendanceShiftId,
    Guid AttendancePolicyId,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsPrimary,
    bool IsActive
) : IRequest<Guid>;
