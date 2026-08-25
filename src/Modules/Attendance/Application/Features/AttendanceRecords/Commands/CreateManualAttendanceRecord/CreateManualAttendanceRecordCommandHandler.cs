using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Services;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.CreateManualAttendanceRecord;

public sealed class CreateManualAttendanceRecordCommandHandler
    : IRequestHandler<CreateManualAttendanceRecordCommand, Guid>
{
    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _assignmentRepository;

    private readonly IReadRepository<AttendanceShift, Guid>
        _shiftRepository;

    private readonly IReadRepository<AttendancePolicy, Guid>
        _policyRepository;

    private readonly IReadRepository<AttendanceRecord, Guid>
        _recordRepository;

    private readonly IWriteRepository<AttendanceRecord, Guid>
        _writeRepository;

    private readonly IAttendanceCalculationService
        _calculationService;

    public CreateManualAttendanceRecordCommandHandler(
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository,
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IWriteRepository<AttendanceRecord, Guid> writeRepository,
        IAttendanceCalculationService calculationService)
    {
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _policyRepository = policyRepository;
        _recordRepository = recordRepository;
        _writeRepository = writeRepository;
        _calculationService = calculationService;
    }

    public async Task<Guid> Handle(
        CreateManualAttendanceRecordCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _recordRepository.FirstOrDefaultAsync(
            x =>
                x.EmployeeId == request.EmployeeId &&
                x.AttendanceDate == request.AttendanceDate &&
                !x.IsDeleted,
            cancellationToken);

        if (existing is not null)
        {
            throw new ConflictException(
                "Attendance record already exists for this employee and date.");
        }

        var assignment =
            (await _assignmentRepository.FindAsync(
                x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.EffectiveFrom <= request.AttendanceDate &&
                    (!x.EffectiveTo.HasValue ||
                     x.EffectiveTo.Value >= request.AttendanceDate),
                cancellationToken))
            .OrderByDescending(x => x.IsPrimary)
            .ThenByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (assignment is null)
        {
            throw new NotFoundException(
                "Employee shift assignment",
                request.EmployeeId);
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

        var record = BuildAttendanceRecord(
            request,
            assignment);

        // Centralized attendance calculation
        _calculationService.Calculate(
            record,
            shift,
            policy);

        await _writeRepository.AddAsync(
            record,
            cancellationToken);

        return record.Id;
    }

    private static AttendanceRecord BuildAttendanceRecord(
        CreateManualAttendanceRecordCommand request,
        EmployeeShiftAssignment assignment)
    {
        return new AttendanceRecord
        {
            Id = Guid.NewGuid(),

            EmployeeId = request.EmployeeId,

            AttendanceShiftId =
                assignment.AttendanceShiftId,

            AttendancePolicyId =
                assignment.AttendancePolicyId,

            AttendanceDate =
                request.AttendanceDate,

            CheckIn =
                request.CheckIn,

            CheckOut =
                request.CheckOut,

            Remarks =
                request.Remarks
        };
    }
}