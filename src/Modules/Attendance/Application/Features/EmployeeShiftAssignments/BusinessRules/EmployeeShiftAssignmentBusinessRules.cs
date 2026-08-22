using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using EmployeeEntity = HRMS.Modules.Employee.Domain.Entities.Employee;

namespace HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.BusinessRules;

public class EmployeeShiftAssignmentBusinessRules
{
    private readonly IReadRepository<EmployeeEntity, Guid> _employeeReadRepository;
    private readonly IReadRepository<AttendanceShift, Guid> _shiftReadRepository;
    private readonly IReadRepository<AttendancePolicy, Guid> _policyReadRepository;
    private readonly IReadRepository<EmployeeShiftAssignment, Guid> _assignmentReadRepository;

    public EmployeeShiftAssignmentBusinessRules(
        IReadRepository<EmployeeEntity, Guid> employeeReadRepository,
        IReadRepository<AttendanceShift, Guid> shiftReadRepository,
        IReadRepository<AttendancePolicy, Guid> policyReadRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentReadRepository)
    {
        _employeeReadRepository = employeeReadRepository;
        _shiftReadRepository = shiftReadRepository;
        _policyReadRepository = policyReadRepository;
        _assignmentReadRepository = assignmentReadRepository;
    }

    public async Task EnsureEmployeeExistsAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await _employeeReadRepository.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException(
                "Employee",
                employeeId);
        }
    }

    public async Task EnsureShiftExistsAsync(
        Guid shiftId,
        CancellationToken cancellationToken = default)
    {
        var shift = await _shiftReadRepository.GetByIdAsync(
            shiftId,
            cancellationToken);

        if (shift is null)
        {
            throw new NotFoundException(
                "Attendance Shift",
                shiftId);
        }
    }

    public async Task EnsurePolicyExistsAsync(
        Guid policyId,
        CancellationToken cancellationToken = default)
    {
        var policy = await _policyReadRepository.GetByIdAsync(
            policyId,
            cancellationToken);

        if (policy is null)
        {
            throw new NotFoundException(
                "Attendance Policy",
                policyId);
        }
    }

    public void EnsureDateRangeIsValid(
        DateOnly effectiveFrom,
        DateOnly? effectiveTo)
    {
        if (effectiveTo.HasValue &&
            effectiveTo.Value < effectiveFrom)
        {
            throw new ConflictException(
                "Effective To date cannot be earlier than Effective From date.");
        }
    }

    public async Task EnsureNoOverlappingAssignmentAsync(
        Guid employeeId,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo,
        Guid? assignmentId = null,
        CancellationToken cancellationToken = default)
    {
        var assignments = await _assignmentReadRepository.FindAsync(
            x =>
                x.EmployeeId == employeeId &&
                !x.IsDeleted &&
                (!assignmentId.HasValue || x.Id != assignmentId.Value),
            cancellationToken);

        var overlaps = assignments.Any(x =>
            effectiveFrom <= (x.EffectiveTo ?? DateOnly.MaxValue) &&
            x.EffectiveFrom <= (effectiveTo ?? DateOnly.MaxValue));

        if (overlaps)
        {
            throw new ConflictException(
                "The employee already has a shift assignment for the selected date range.");
        }
    }

    public async Task EnsurePrimaryAssignmentUniqueAsync(
        Guid employeeId,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo,
        Guid? assignmentId = null,
        CancellationToken cancellationToken = default)
    {
        var assignments = await _assignmentReadRepository.FindAsync(
            x =>
                x.EmployeeId == employeeId &&
                x.IsPrimary &&
                x.IsActive &&
                !x.IsDeleted &&
                (!assignmentId.HasValue || x.Id != assignmentId.Value),
            cancellationToken);

        var overlaps = assignments.Any(x =>
            effectiveFrom <= (x.EffectiveTo ?? DateOnly.MaxValue) &&
            x.EffectiveFrom <= (effectiveTo ?? DateOnly.MaxValue));

        if (overlaps)
        {
            throw new ConflictException(
                "The employee already has a primary shift assignment for the selected date range.");
        }
    }
}
