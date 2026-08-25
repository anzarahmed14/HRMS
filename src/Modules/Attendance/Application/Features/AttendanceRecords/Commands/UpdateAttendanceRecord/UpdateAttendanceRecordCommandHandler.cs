using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Services;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.UpdateAttendanceRecord;

public sealed class UpdateAttendanceRecordCommandHandler
    : IRequestHandler<UpdateAttendanceRecordCommand>
{
    private readonly IReadRepository<AttendanceRecord, Guid>
        _recordRepository;

    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _assignmentRepository;

    private readonly IReadRepository<AttendanceShift, Guid>
        _shiftRepository;

    private readonly IReadRepository<AttendancePolicy, Guid>
        _policyRepository;

    private readonly IWriteRepository<AttendanceRecord, Guid>
        _writeRepository;

    private readonly IAttendanceCalculationService
        _calculationService;

    public UpdateAttendanceRecordCommandHandler(
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository,
        IWriteRepository<AttendanceRecord, Guid> writeRepository,
        IAttendanceCalculationService calculationService)
    {
        _recordRepository = recordRepository;
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _policyRepository = policyRepository;
        _writeRepository = writeRepository;
        _calculationService = calculationService;
    }

    public async Task Handle(
        UpdateAttendanceRecordCommand request,
        CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (record is null || record.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance record",
                request.Id);
        }

        var assignment =
            (await _assignmentRepository.FindAsync(
                x =>
                    x.EmployeeId == record.EmployeeId &&
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.EffectiveFrom <= record.AttendanceDate &&
                    (!x.EffectiveTo.HasValue ||
                     x.EffectiveTo.Value >= record.AttendanceDate),
                cancellationToken))
            .OrderByDescending(x => x.IsPrimary)
            .ThenByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (assignment is null)
        {
            throw new NotFoundException(
                "Employee shift assignment",
                record.EmployeeId);
        }

        var shift = await _shiftRepository.GetByIdAsync(
            assignment.AttendanceShiftId,
            cancellationToken);

        if (shift is null ||
            shift.IsDeleted ||
            !shift.IsActive)
        {
            throw new NotFoundException(
                "Attendance shift",
                assignment.AttendanceShiftId);
        }

        var policy = await _policyRepository.GetByIdAsync(
            assignment.AttendancePolicyId,
            cancellationToken);

        if (policy is null ||
            policy.IsDeleted ||
            !policy.IsActive)
        {
            throw new NotFoundException(
                "Attendance policy",
                assignment.AttendancePolicyId);
        }

        // ---------------------------------------------------------
        // Update requested attendance values
        // ---------------------------------------------------------

        record.CheckIn = request.CheckIn;
        record.CheckOut = request.CheckOut;
        record.Remarks = request.Remarks;

        // ---------------------------------------------------------
        // Centralized attendance calculation
        // ---------------------------------------------------------

        _calculationService.Calculate(
            record,
            shift,
            policy);

        // ---------------------------------------------------------
        // Persist updated attendance record
        // ---------------------------------------------------------

        await _writeRepository.UpdateAsync(
            record,
            cancellationToken);
    }
}