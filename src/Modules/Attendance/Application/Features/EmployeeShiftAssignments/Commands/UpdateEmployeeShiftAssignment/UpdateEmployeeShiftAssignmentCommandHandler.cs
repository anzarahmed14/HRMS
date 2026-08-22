using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.UpdateEmployeeShiftAssignment;

public sealed class UpdateEmployeeShiftAssignmentCommandHandler
    : IRequestHandler<UpdateEmployeeShiftAssignmentCommand>
{
    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _readRepository;

    private readonly IWriteRepository<EmployeeShiftAssignment, Guid>
        _writeRepository;

    private readonly EmployeeShiftAssignmentBusinessRules
        _businessRules;

    public UpdateEmployeeShiftAssignmentCommandHandler(
        IReadRepository<EmployeeShiftAssignment, Guid> readRepository,
        IWriteRepository<EmployeeShiftAssignment, Guid> writeRepository,
        EmployeeShiftAssignmentBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateEmployeeShiftAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (assignment is null || assignment.IsDeleted)
        {
            throw new NotFoundException(
                "Employee Shift Assignment",
                request.Id);
        }

        await _businessRules.EnsureShiftExistsAsync(
            request.AttendanceShiftId,
            cancellationToken);

        await _businessRules.EnsurePolicyExistsAsync(
            request.AttendancePolicyId,
            cancellationToken);

        _businessRules.EnsureDateRangeIsValid(
            request.EffectiveFrom,
            request.EffectiveTo);

        await _businessRules.EnsureNoOverlappingAssignmentAsync(
            assignment.EmployeeId,
            request.EffectiveFrom,
            request.EffectiveTo,
            request.Id,
            cancellationToken);

        if (request.IsPrimary && request.IsActive)
        {
            await _businessRules.EnsurePrimaryAssignmentUniqueAsync(
                assignment.EmployeeId,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.Id,
                cancellationToken);
        }

        assignment.AttendanceShiftId =
            request.AttendanceShiftId;

        assignment.AttendancePolicyId =
            request.AttendancePolicyId;

        assignment.EffectiveFrom =
            request.EffectiveFrom;

        assignment.EffectiveTo =
            request.EffectiveTo;

        assignment.IsPrimary =
            request.IsPrimary;

        assignment.IsActive =
            request.IsActive;

        await _writeRepository.UpdateAsync(
            assignment,
            cancellationToken);
    }
}
