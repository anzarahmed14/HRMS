using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;

public sealed class CreateLeaveRequestCommandHandler
    : IRequestHandler<CreateLeaveRequestCommand, Guid>
{
    private readonly IWriteRepository<LeaveRequest, Guid> _writeRepository;
    private readonly LeaveRequestBusinessRules _businessRules;

    public CreateLeaveRequestCommandHandler(
        IWriteRepository<LeaveRequest, Guid> writeRepository,
        LeaveRequestBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate employee
        await _businessRules.EnsureEmployeeIsValidAsync(
            request.EmployeeId,
            cancellationToken);

        // 2. Validate leave year
        var leaveYear =
            await _businessRules.EnsureLeaveYearIsValidAsync(
                request.LeaveYearId,
                cancellationToken);

        // 3. Validate leave type
        await _businessRules.EnsureLeaveTypeIsValidAsync(
            request.LeaveTypeId,
            cancellationToken);

        // 4. Validate start day part
        var startDayPart =
            await _businessRules.EnsureLeaveDayPartIsValidAsync(
                request.StartDayPartId,
                cancellationToken);

        // 5. Validate end day part
        var endDayPart =
            await _businessRules.EnsureLeaveDayPartIsValidAsync(
                request.EndDayPartId,
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

        // 8. Prevent overlapping submitted/approved leave
        await _businessRules.EnsureNoOverlappingLeaveRequestAsync(
            request.EmployeeId,
            request.FromDate,
            request.ToDate,
            cancellationToken: cancellationToken);

        // 9. Calculate total days
        var totalDays =
            _businessRules.CalculateTotalDays(
                request.FromDate,
                request.ToDate,
                startDayPart,
                endDayPart);

        // 10. New request starts as DRAFT
        var draftStatusId =
            await _businessRules.GetDraftStatusIdAsync(
                cancellationToken);

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = request.EmployeeId,
            LeaveYearId = request.LeaveYearId,
            LeaveTypeId = request.LeaveTypeId,
            StartDayPartId = request.StartDayPartId,
            EndDayPartId = request.EndDayPartId,
            StatusId = draftStatusId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            TotalDays = totalDays,
            Reason = request.Reason.Trim()
        };

        await _writeRepository.AddAsync(
            leaveRequest,
            cancellationToken);

        return leaveRequest.Id;
    }
}