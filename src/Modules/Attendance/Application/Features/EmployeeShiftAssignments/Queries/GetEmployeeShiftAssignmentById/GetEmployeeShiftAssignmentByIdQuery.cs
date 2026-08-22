using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Queries.GetEmployeeShiftAssignmentById;

public sealed record GetEmployeeShiftAssignmentByIdQuery(
    Guid Id
) : IRequest<EmployeeShiftAssignmentDto>;
