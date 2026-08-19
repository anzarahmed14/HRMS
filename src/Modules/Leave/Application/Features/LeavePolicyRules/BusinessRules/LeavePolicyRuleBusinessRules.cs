using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.BusinessRules;

public class LeavePolicyRuleBusinessRules
{
    private readonly IReadRepository<LeavePolicyRule, Guid>
        _leavePolicyRuleRepository;

    private readonly IReadRepository<LeavePolicy, Guid>
        _leavePolicyRepository;

    private readonly IReadRepository<LeaveType, Guid>
        _leaveTypeRepository;

    public LeavePolicyRuleBusinessRules(
        IReadRepository<LeavePolicyRule, Guid> leavePolicyRuleRepository,
        IReadRepository<LeavePolicy, Guid> leavePolicyRepository,
        IReadRepository<LeaveType, Guid> leaveTypeRepository)
    {
        _leavePolicyRuleRepository = leavePolicyRuleRepository;
        _leavePolicyRepository = leavePolicyRepository;
        _leaveTypeRepository = leaveTypeRepository;
    }

    public async Task EnsurePolicyRuleExistsAsync(
        Guid ruleId,
        CancellationToken cancellationToken = default)
    {
        var rule = await _leavePolicyRuleRepository.GetByIdAsync(
            ruleId,
            cancellationToken);

        if (rule is null)
        {
            throw new NotFoundException(
                "Leave Policy Rule",
                ruleId);
        }
    }

    public async Task EnsurePolicyExistsAsync(
        Guid policyId,
        CancellationToken cancellationToken = default)
    {
        var policy = await _leavePolicyRepository.GetByIdAsync(
            policyId,
            cancellationToken);

        if (policy is null)
        {
            throw new NotFoundException(
                "Leave Policy",
                policyId);
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

    public async Task EnsureUniquePolicyLeaveTypeAsync(
        Guid leavePolicyId,
        Guid leaveTypeId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _leavePolicyRuleRepository.AnyAsync(
            x =>
                x.LeavePolicyId == leavePolicyId &&
                x.LeaveTypeId == leaveTypeId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This leave type is already configured for the leave policy.");
        }
    }

    public async Task EnsureUniquePolicyLeaveTypeAsync(
        Guid leavePolicyId,
        Guid leaveTypeId,
        Guid ruleId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _leavePolicyRuleRepository.AnyAsync(
            x =>
                x.LeavePolicyId == leavePolicyId &&
                x.LeaveTypeId == leaveTypeId &&
                x.Id != ruleId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This leave type is already configured for the leave policy.");
        }
    }

    public void EnsureAnnualEntitlementIsValid(
        decimal annualEntitlement)
    {
        if (annualEntitlement < 0)
        {
            throw new ConflictException(
                "Annual entitlement cannot be negative.");
        }
    }
}