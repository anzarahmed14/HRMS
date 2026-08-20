using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.RejectLeaveRequest;

public sealed class RejectLeaveRequestCommandHandler
    : IRequestHandler<RejectLeaveRequestCommand>
{
    private readonly IReadRepository<LeaveRequest, Guid> _readRepository;
    private readonly IWriteRepository<LeaveRequest, Guid> _writeRepository;
    private readonly LeaveRequestBusinessRules _businessRules;

    public RejectLeaveRequestCommandHandler(
        IReadRepository<LeaveRequest, Guid> readRepository,
        IWriteRepository<LeaveRequest, Guid> writeRepository,
        LeaveRequestBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        RejectLeaveRequestCommand request,
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

        // 2. Only PENDING requests can be rejected
        await _businessRules.EnsureCanRejectAsync(
            leaveRequest,
            cancellationToken);

        // 3. Get REJECTED status
        var rejectedStatusId =
            await _businessRules.GetRejectedStatusIdAsync(
                cancellationToken);

        // 4. Change status
        leaveRequest.StatusId = rejectedStatusId;

        // 5. Capture rejection information
        leaveRequest.RejectedOn = DateTimeOffset.UtcNow;
        leaveRequest.RejectionReason =
            request.RejectionReason.Trim();

        // 6. Save
        //
        // No balance transaction is required because
        // PENDING leave has not consumed any balance.
        await _writeRepository.UpdateAsync(
            leaveRequest,
            cancellationToken);
    }
}
