using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Abstractions.Persistence;
using HRMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Transactions;

public sealed class LeaveBalanceTransaction : ILeaveBalanceTransaction
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWorkTransaction _transaction;

    public LeaveBalanceTransaction(
        ApplicationDbContext context,
        IUnitOfWorkTransaction transaction)
    {
        _context = context;
        _transaction = transaction;
    }

    public async Task ApproveLeaveAsync(
        Guid leaveRequestId,
        decimal days,
        Guid approvedStatusId,
        DateTimeOffset approvedOn,
        string? approvalReason,
        CancellationToken cancellationToken = default)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(
                x =>
                    x.Id == leaveRequestId &&
                    !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
        {
            throw new NotFoundException(
                "Leave Request",
                leaveRequestId);
        }

        var entitlement = await GetEntitlementAsync(
            leaveRequest,
            cancellationToken);

        var availableDays =
            entitlement.EntitledDays - entitlement.UsedDays;

        if (availableDays < days)
        {
            throw new ConflictException(
                $"Insufficient leave balance. " +
                $"Requested: {days:0.##} days, " +
                $"Available: {availableDays:0.##} days.");
        }

        await _transaction.BeginAsync(cancellationToken);

        try
        {
            entitlement.UsedDays += days;

            leaveRequest.StatusId = approvedStatusId;
            leaveRequest.ApprovedOn = approvedOn;
            leaveRequest.ApprovalReason = approvalReason;

            await _context.SaveChangesAsync(cancellationToken);

            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task CancelLeaveAsync(
        Guid leaveRequestId,
        decimal days,
        Guid cancelledStatusId,
        DateTimeOffset cancelledOn,
        string cancellationReason,
        CancellationToken cancellationToken = default)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(
                x =>
                    x.Id == leaveRequestId &&
                    !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
        {
            throw new NotFoundException(
                "Leave Request",
                leaveRequestId);
        }

        var entitlement = await GetEntitlementAsync(
            leaveRequest,
            cancellationToken);

        if (entitlement.UsedDays < days)
        {
            throw new ConflictException(
                "Leave balance cannot be restored because the used balance is insufficient.");
        }

        await _transaction.BeginAsync(cancellationToken);

        try
        {
            entitlement.UsedDays -= days;

            leaveRequest.StatusId = cancelledStatusId;
            leaveRequest.CancelledOn = cancelledOn;
            leaveRequest.CancellationReason = cancellationReason;

            await _context.SaveChangesAsync(cancellationToken);

            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<HRMS.Modules.Leave.Domain.Entities.EmployeeLeaveEntitlement>
        GetEntitlementAsync(
            HRMS.Modules.Leave.Domain.Entities.LeaveRequest leaveRequest,
            CancellationToken cancellationToken)
    {
        var entitlement =
            await _context.EmployeeLeaveEntitlements
                .FirstOrDefaultAsync(
                    x =>
                        x.EmployeeId == leaveRequest.EmployeeId &&
                        x.LeaveYearId == leaveRequest.LeaveYearId &&
                        x.LeaveTypeId == leaveRequest.LeaveTypeId &&
                        !x.IsDeleted,
                    cancellationToken);

        if (entitlement is null)
        {
            throw new NotFoundException(
                "Employee Leave Entitlement",
                leaveRequest.EmployeeId);
        }

        return entitlement;
    }
}
