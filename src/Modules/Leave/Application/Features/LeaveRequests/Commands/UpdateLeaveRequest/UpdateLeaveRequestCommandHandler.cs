using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;

public sealed class UpdateLeaveRequestCommandHandler
    : IRequestHandler<UpdateLeaveRequestCommand>
{
    private readonly IReadRepository<LeaveRequest, Guid> _readRepository;
    private readonly IWriteRepository<LeaveRequest, Guid> _writeRepository;
    private readonly LeaveRequestBusinessRules _businessRules;

    public UpdateLeaveRequestCommandHandler(
        IReadRepository<LeaveRequest, Guid> readRepository,
        IWriteRepository<LeaveRequest, Guid> writeRepository,
        LeaveRequestBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateLeaveRequestCommand request,
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

        // 2. Only DRAFT requests can be updated
        await _businessRules.EnsureCanUpdateAsync(
            leaveRequest,
            cancellationToken);

        // 3. Validate leave type
        await _businessRules.EnsureLeaveTypeIsValidAsync(
            request.LeaveTypeId,
            cancellationToken);

        // 4. Validate day parts
        var startDayPart =
            await _businessRules.EnsureLeaveDayPartIsValidAsync(
                request.StartDayPartId,
                cancellationToken);

        var endDayPart =
            await _businessRules.EnsureLeaveDayPartIsValidAsync(
                request.EndDayPartId,
                cancellationToken);

        // 5. Validate leave year
        var leaveYear =
            await _businessRules.EnsureLeaveYearIsValidAsync(
                leaveRequest.LeaveYearId,
                cancellationToken);

        // 6. Validate dates
        _businessRules.EnsureDatesAreValid(
            request.FromDate,
            request.ToDate,
            leaveYear);

        // 7. Validate day-part combination
        _businessRules.EnsureDayPartsAreValid(
            request.FromDate,
            request.ToDate,
            startDayPart,
            endDayPart);

        // 8. Check overlapping PENDING / APPROVED leave
        await _businessRules.EnsureNoOverlappingLeaveRequestAsync(
            leaveRequest.EmployeeId,
            request.FromDate,
            request.ToDate,
            excludeLeaveRequestId: request.Id,
            cancellationToken: cancellationToken);

        // 9. Recalculate total days
        var totalDays =
            await _businessRules.CalculateTotalDaysAsync(
                leaveRequest.LeaveYearId,
                request.FromDate,
                request.ToDate,
                startDayPart,
                endDayPart,
                cancellationToken);

        // 10. Update editable fields
        leaveRequest.LeaveTypeId = request.LeaveTypeId;
        leaveRequest.StartDayPartId = request.StartDayPartId;
        leaveRequest.EndDayPartId = request.EndDayPartId;
        leaveRequest.FromDate = request.FromDate;
        leaveRequest.ToDate = request.ToDate;
        leaveRequest.TotalDays = totalDays;
        leaveRequest.Reason = request.Reason.Trim();

        // 11. Save
        await _writeRepository.UpdateAsync(
            leaveRequest,
            cancellationToken);
    }
}

