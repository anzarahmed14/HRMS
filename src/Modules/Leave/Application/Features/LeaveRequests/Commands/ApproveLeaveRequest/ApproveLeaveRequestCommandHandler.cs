using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest;

public sealed class ApproveLeaveRequestCommandHandler
    : IRequestHandler<ApproveLeaveRequestCommand>
{
    private readonly IReadRepository<LeaveRequest, Guid> _readRepository;
    private readonly LeaveRequestBusinessRules _businessRules;
    private readonly ILeaveBalanceTransaction _leaveBalanceTransaction;

    public ApproveLeaveRequestCommandHandler(
        IReadRepository<LeaveRequest, Guid> readRepository,
        LeaveRequestBusinessRules businessRules,
        ILeaveBalanceTransaction leaveBalanceTransaction)
    {
        _readRepository = readRepository;
        _businessRules = businessRules;
        _leaveBalanceTransaction = leaveBalanceTransaction;
    }

    public async Task Handle(
        ApproveLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Get leave request
        var leaveRequest = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (leaveRequest is null || leaveRequest.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Request",
                request.Id);
        }

        // 2. Only PENDING requests can be approved
        await _businessRules.EnsureCanApproveAsync(
            leaveRequest,
            cancellationToken);

        // 3. Get APPROVED status
        var approvedStatusId =
            await _businessRules.GetApprovedStatusIdAsync(
                cancellationToken);

        // 4. Approve request and deduct leave balance.
        //
        // LeaveRequest and EmployeeLeaveEntitlement
        // are updated inside the same transaction.
        await _leaveBalanceTransaction.ApproveLeaveAsync(
            leaveRequest.Id,
            leaveRequest.TotalDays,
            approvedStatusId,
            DateTimeOffset.UtcNow,
            request.ApprovalReason?.Trim(),
            cancellationToken);
    }
}
