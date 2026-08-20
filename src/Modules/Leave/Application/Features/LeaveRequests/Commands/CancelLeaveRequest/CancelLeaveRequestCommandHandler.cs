using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;

public sealed class CancelLeaveRequestCommandHandler
    : IRequestHandler<CancelLeaveRequestCommand>
{
    private readonly IReadRepository<LeaveRequest, Guid> _readRepository;
    private readonly LeaveRequestBusinessRules _businessRules;
    private readonly ILeaveBalanceTransaction _leaveBalanceTransaction;

    public CancelLeaveRequestCommandHandler(
        IReadRepository<LeaveRequest, Guid> readRepository,
        LeaveRequestBusinessRules businessRules,
        ILeaveBalanceTransaction leaveBalanceTransaction)
    {
        _readRepository = readRepository;
        _businessRules = businessRules;
        _leaveBalanceTransaction = leaveBalanceTransaction;
    }

    public async Task Handle(
        CancelLeaveRequestCommand request,
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

        // 2. Only APPROVED requests can be cancelled
        await _businessRules.EnsureCanCancelAsync(
            leaveRequest,
            cancellationToken);

        // 3. Get CANCELLED status
        var cancelledStatusId =
            await _businessRules.GetCancelledStatusIdAsync(
                cancellationToken);

        // 4. Cancel request and restore leave balance.
        //
        // LeaveRequest and EmployeeLeaveEntitlement
        // are updated inside the same transaction.
        await _leaveBalanceTransaction.CancelLeaveAsync(
            leaveRequest.Id,
            leaveRequest.TotalDays,
            cancelledStatusId,
            DateTimeOffset.UtcNow,
            request.CancellationReason.Trim(),
            cancellationToken);
    }
}
