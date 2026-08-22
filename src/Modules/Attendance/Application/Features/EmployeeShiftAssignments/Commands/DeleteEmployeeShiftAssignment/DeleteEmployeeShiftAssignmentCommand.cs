using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.DeleteEmployeeShiftAssignment;

public sealed record DeleteEmployeeShiftAssignmentCommand(
    Guid Id
) : IRequest;
