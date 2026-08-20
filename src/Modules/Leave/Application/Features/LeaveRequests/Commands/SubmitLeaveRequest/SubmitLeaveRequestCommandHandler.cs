using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.SubmitLeaveRequest;

public sealed class SubmitLeaveRequestCommandHandler
    : IRequestHandler<SubmitLeaveRequestCommand>
{
    private readonly IReadRepository<LeaveRequest, Guid> _readRepository;
    private readonly IWriteRepository<LeaveRequest, Guid> _writeRepository;
    private readonly LeaveRequestBusinessRules _businessRules;

    public SubmitLeaveRequestCommandHandler(
        IReadRepository<LeaveRequest, Guid> readRepository,
        IWriteRepository<LeaveRequest, Guid> writeRepository,
        LeaveRequestBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        SubmitLeaveRequestCommand request,
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

        // 2. Only DRAFT requests can be submitted
        await _businessRules.EnsureCanSubmitAsync(
            leaveRequest,
            cancellationToken);

        // 3. Get PENDING status
        var pendingStatusId =
            await _businessRules.GetPendingStatusIdAsync(
                cancellationToken);

        // 4. Revalidate employee
        await _businessRules.EnsureEmployeeIsValidAsync(
            leaveRequest.EmployeeId,
            cancellationToken);

        // 5. Revalidate leave year
        var leaveYear =
            await _businessRules.EnsureLeaveYearIsValidAsync(
                leaveRequest.LeaveYearId,
                cancellationToken);

        // 6. Revalidate leave type
        await _businessRules.EnsureLeaveTypeIsValidAsync(
            leaveRequest.LeaveTypeId,
            cancellationToken);

        // 7. Revalidate day parts
        var startDayPart =
            await _businessRules.EnsureLeaveDayPartIsValidAsync(
                leaveRequest.StartDayPartId,
                cancellationToken);

        var endDayPart =
            await _businessRules.EnsureLeaveDayPartIsValidAsync(
                leaveRequest.EndDayPartId,
                cancellationToken);

        // 8. Validate dates
        _businessRules.EnsureDatesAreValid(
            leaveRequest.FromDate,
            leaveRequest.ToDate,
            leaveYear);

        // 9. Validate day-part combination
        _businessRules.EnsureDayPartsAreValid(
            leaveRequest.FromDate,
            leaveRequest.ToDate,
            startDayPart,
            endDayPart);

        // 10. Recalculate total days
        var totalDays =
            _businessRules.CalculateTotalDays(
                leaveRequest.FromDate,
                leaveRequest.ToDate,
                startDayPart,
                endDayPart);

        // 11. Check overlapping PENDING / APPROVED leave
        await _businessRules.EnsureNoOverlappingLeaveRequestAsync(
            leaveRequest.EmployeeId,
            leaveRequest.FromDate,
            leaveRequest.ToDate,
            excludeLeaveRequestId: leaveRequest.Id,
            cancellationToken: cancellationToken);

        // 12. Update server-controlled values
        leaveRequest.TotalDays = totalDays;

        // 13. DRAFT ? PENDING
        leaveRequest.StatusId = pendingStatusId;

        // 14. Save
        await _writeRepository.UpdateAsync(
            leaveRequest,
            cancellationToken);
    }
}
