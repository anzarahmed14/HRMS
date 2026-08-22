using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Queries.GetEmployeeShiftAssignmentById;

public sealed class GetEmployeeShiftAssignmentByIdQueryHandler
    : IRequestHandler<
        GetEmployeeShiftAssignmentByIdQuery,
        EmployeeShiftAssignmentDto>
{
    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _repository;

    public GetEmployeeShiftAssignmentByIdQueryHandler(
        IReadRepository<EmployeeShiftAssignment, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeShiftAssignmentDto> Handle(
        GetEmployeeShiftAssignmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var assignment = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (assignment is null || assignment.IsDeleted)
        {
            throw new NotFoundException(
                "Employee Shift Assignment",
                request.Id);
        }

        return new EmployeeShiftAssignmentDto
        {
            Id = assignment.Id,
            EmployeeId = assignment.EmployeeId,
            AttendanceShiftId = assignment.AttendanceShiftId,
            AttendancePolicyId = assignment.AttendancePolicyId,
            EffectiveFrom = assignment.EffectiveFrom,
            EffectiveTo = assignment.EffectiveTo,
            IsPrimary = assignment.IsPrimary,
            IsActive = assignment.IsActive
        };
    }
}
