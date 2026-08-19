using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.BusinessRules;

public class EmployeeLeaveEntitlementBusinessRules
{
    private readonly IReadRepository<EmployeeLeaveEntitlement, Guid>
        _entitlementRepository;

    private readonly IReadRepository<LeaveYear, Guid>
        _leaveYearRepository;

    private readonly IReadRepository<LeaveType, Guid>
        _leaveTypeRepository;

    private readonly IReadRepository<LeavePolicyRule, Guid>
        _leavePolicyRuleRepository;

    public EmployeeLeaveEntitlementBusinessRules(
        IReadRepository<EmployeeLeaveEntitlement, Guid> entitlementRepository,
        IReadRepository<LeaveYear, Guid> leaveYearRepository,
        IReadRepository<LeaveType, Guid> leaveTypeRepository,
        IReadRepository<LeavePolicyRule, Guid> leavePolicyRuleRepository)
    {
        _entitlementRepository = entitlementRepository;
        _leaveYearRepository = leaveYearRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _leavePolicyRuleRepository = leavePolicyRuleRepository;
    }

    public async Task EnsureEntitlementExistsAsync(
        Guid entitlementId,
        CancellationToken cancellationToken = default)
    {
        var entitlement = await _entitlementRepository.GetByIdAsync(
            entitlementId,
            cancellationToken);

        if (entitlement is null)
        {
            throw new NotFoundException(
                "Employee Leave Entitlement",
                entitlementId);
        }
    }

    public async Task EnsurePolicyRuleMatchesLeaveTypeAsync(
    Guid leavePolicyRuleId,
    Guid leaveTypeId,
    CancellationToken cancellationToken = default)
    {
        var rule = await _leavePolicyRuleRepository.GetByIdAsync(
            leavePolicyRuleId,
            cancellationToken);

        if (rule is null)
        {
            throw new NotFoundException(
                "Leave Policy Rule",
                leavePolicyRuleId);
        }

        if (rule.LeaveTypeId != leaveTypeId)
        {
            throw new ConflictException(
                "The leave policy rule does not belong to the selected leave type.");
        }
    }
    public async Task EnsureLeaveYearExistsAsync(
        Guid leaveYearId,
        CancellationToken cancellationToken = default)
    {
        var leaveYear = await _leaveYearRepository.GetByIdAsync(
            leaveYearId,
            cancellationToken);

        if (leaveYear is null)
        {
            throw new NotFoundException(
                "Leave Year",
                leaveYearId);
        }
    }

    public async Task EnsureLeaveTypeExistsAsync(
        Guid leaveTypeId,
        CancellationToken cancellationToken = default)
    {
        var leaveType = await _leaveTypeRepository.GetByIdAsync(
            leaveTypeId,
            cancellationToken);

        if (leaveType is null)
        {
            throw new NotFoundException(
                "Leave Type",
                leaveTypeId);
        }
    }

    public async Task EnsurePolicyRuleExistsAsync(
        Guid leavePolicyRuleId,
        CancellationToken cancellationToken = default)
    {
        var rule = await _leavePolicyRuleRepository.GetByIdAsync(
            leavePolicyRuleId,
            cancellationToken);

        if (rule is null)
        {
            throw new NotFoundException(
                "Leave Policy Rule",
                leavePolicyRuleId);
        }
    }

    public async Task EnsureUniqueEmployeeLeaveEntitlementAsync(
        Guid employeeId,
        Guid leaveYearId,
        Guid leaveTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _entitlementRepository.AnyAsync(
            x =>
                x.EmployeeId == employeeId &&
                x.LeaveYearId == leaveYearId &&
                x.LeaveTypeId == leaveTypeId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Leave entitlement already exists for this employee, leave year and leave type.");
        }
    }

    public async Task EnsureUniqueEmployeeLeaveEntitlementAsync(
        Guid employeeId,
        Guid leaveYearId,
        Guid leaveTypeId,
        Guid entitlementId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _entitlementRepository.AnyAsync(
            x =>
                x.EmployeeId == employeeId &&
                x.LeaveYearId == leaveYearId &&
                x.LeaveTypeId == leaveTypeId &&
                x.Id != entitlementId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Leave entitlement already exists for this employee, leave year and leave type.");
        }
    }

    public void EnsureEntitledDaysAreValid(
        decimal entitledDays)
    {
        if (entitledDays < 0)
        {
            throw new ConflictException(
                "Entitled days cannot be negative.");
        }
    }
}