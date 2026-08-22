using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.CreateEmployeeShiftAssignment;

public sealed class CreateEmployeeShiftAssignmentCommandHandler
    : IRequestHandler<CreateEmployeeShiftAssignmentCommand, Guid>
{
    private readonly IWriteRepository<EmployeeShiftAssignment, Guid>
        _writeRepository;

    private readonly EmployeeShiftAssignmentBusinessRules
        _businessRules;

    public CreateEmployeeShiftAssignmentCommandHandler(
        IWriteRepository<EmployeeShiftAssignment, Guid> writeRepository,
        EmployeeShiftAssignmentBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeShiftAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureShiftExistsAsync(
            request.AttendanceShiftId,
            cancellationToken);

        await _businessRules.EnsurePolicyExistsAsync(
            request.AttendancePolicyId,
            cancellationToken);

        _businessRules.EnsureDateRangeIsValid(
            request.EffectiveFrom,
            request.EffectiveTo);

        var assignment = new EmployeeShiftAssignment
        {
            EmployeeId = request.EmployeeId,
            AttendanceShiftId = request.AttendanceShiftId,
            AttendancePolicyId = request.AttendancePolicyId,
            EffectiveFrom = request.EffectiveFrom,
            EffectiveTo = request.EffectiveTo,
            IsPrimary = request.IsPrimary,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            assignment,
            cancellationToken);

        return assignment.Id;
    }
}
