using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Services;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.ApproveAttendanceRegularization;

public sealed class ApproveAttendanceRegularizationCommandHandler
    : IRequestHandler<ApproveAttendanceRegularizationCommand>
{
    private static readonly Guid PendingStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000101");

    private static readonly Guid ApprovedStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000102");

    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IReadRepository<AttendanceRecord, Guid>
        _recordRepository;

    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _assignmentRepository;

    private readonly IReadRepository<AttendanceShift, Guid>
        _shiftRepository;

    private readonly IReadRepository<AttendancePolicy, Guid>
        _policyRepository;

    private readonly IWriteRepository<AttendanceRegularization, Guid>
        _regularizationWriteRepository;

    private readonly IWriteRepository<AttendanceRecord, Guid>
        _recordWriteRepository;

    private readonly IUserContext _userContext;

    private readonly IAttendanceCalculationService
        _calculationService;

    public ApproveAttendanceRegularizationCommandHandler(
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IReadRepository<AttendanceRecord, Guid> recordRepository,
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceShift, Guid> shiftRepository,
        IReadRepository<AttendancePolicy, Guid> policyRepository,
        IWriteRepository<AttendanceRegularization, Guid> regularizationWriteRepository,
        IWriteRepository<AttendanceRecord, Guid> recordWriteRepository,
        IUserContext userContext,
        IAttendanceCalculationService calculationService)
    {
        _regularizationRepository =
            regularizationRepository;

        _recordRepository =
            recordRepository;

        _assignmentRepository =
            assignmentRepository;

        _shiftRepository =
            shiftRepository;

        _policyRepository =
            policyRepository;

        _regularizationWriteRepository =
            regularizationWriteRepository;

        _recordWriteRepository =
            recordWriteRepository;

        _userContext =
            userContext;

        _calculationService =
            calculationService;
    }

    public async Task Handle(
        ApproveAttendanceRegularizationCommand request,
        CancellationToken cancellationToken)
    {
        // ---------------------------------------------------------
        // 1. GET REGULARIZATION
        // ---------------------------------------------------------

        var regularization =
            await _regularizationRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (regularization is null ||
            regularization.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance regularization",
                request.Id);
        }

        // ---------------------------------------------------------
        // 2. ONLY PENDING CAN BE APPROVED
        // ---------------------------------------------------------

        if (regularization.AttendanceRegularizationStatusId !=
            PendingStatusId)
        {
            throw new ConflictException(
                "Only pending attendance regularizations can be approved.");
        }

        // ---------------------------------------------------------
        // 3. GET ATTENDANCE RECORD
        // ---------------------------------------------------------

        var record =
            await _recordRepository.GetByIdAsync(
                regularization.AttendanceRecordId,
                cancellationToken);

        if (record is null ||
            record.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance record",
                regularization.AttendanceRecordId);
        }

        // ---------------------------------------------------------
        // 4. GET EMPLOYEE SHIFT ASSIGNMENT
        // ---------------------------------------------------------

        var assignments =
            await _assignmentRepository.FindAsync(
                x =>
                    x.EmployeeId ==
                        regularization.EmployeeId &&
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.EffectiveFrom <=
                        regularization.AttendanceDate &&
                    (!x.EffectiveTo.HasValue ||
                     x.EffectiveTo.Value >=
                        regularization.AttendanceDate),
                cancellationToken);

        var assignment = assignments
            .OrderByDescending(x => x.IsPrimary)
            .ThenByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (assignment is null)
        {
            throw new NotFoundException(
                "Employee shift assignment",
                regularization.EmployeeId);
        }

        // ---------------------------------------------------------
        // 5. GET SHIFT
        // ---------------------------------------------------------

        var shift =
            await _shiftRepository.GetByIdAsync(
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

        // ---------------------------------------------------------
        // 6. GET ATTENDANCE POLICY
        // ---------------------------------------------------------

        var policy =
            await _policyRepository.GetByIdAsync(
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
        // 7. APPLY REQUESTED ATTENDANCE
        // ---------------------------------------------------------

        record.CheckIn =
            regularization.RequestedCheckIn;

        record.CheckOut =
            regularization.RequestedCheckOut;

        // ---------------------------------------------------------
        // 8. CALCULATE ATTENDANCE
        // ---------------------------------------------------------

        _calculationService.Calculate(
            record,
            shift,
            policy);

        // ---------------------------------------------------------
        // 9. APPROVE REGULARIZATION
        // ---------------------------------------------------------

        regularization.AttendanceRegularizationStatusId =
            ApprovedStatusId;

        regularization.ApprovedBy =
            _userContext.UserId;

        regularization.ApprovedOn =
            DateTimeOffset.UtcNow;

        regularization.ApprovalRemarks =
            request.Remarks;

        // ---------------------------------------------------------
        // 10. SAVE ATTENDANCE RECORD
        // ---------------------------------------------------------

        await _recordWriteRepository.UpdateAsync(
            record,
            cancellationToken);

        // ---------------------------------------------------------
        // 11. SAVE REGULARIZATION
        // ---------------------------------------------------------

        await _regularizationWriteRepository.UpdateAsync(
            regularization,
            cancellationToken);
    }
}