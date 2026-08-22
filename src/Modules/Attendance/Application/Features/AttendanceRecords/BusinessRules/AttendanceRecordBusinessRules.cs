using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.BusinessRules;

public class AttendanceRecordBusinessRules
{
    private readonly IReadRepository<EmployeeShiftAssignment, Guid>
        _assignmentRepository;

    private readonly IReadRepository<AttendanceRecord, Guid>
        _attendanceRecordRepository;

    public AttendanceRecordBusinessRules(
        IReadRepository<EmployeeShiftAssignment, Guid> assignmentRepository,
        IReadRepository<AttendanceRecord, Guid> attendanceRecordRepository)
    {
        _assignmentRepository = assignmentRepository;
        _attendanceRecordRepository = attendanceRecordRepository;
    }

    public async Task EnsureAssignmentExistsAsync(
        Guid employeeId,
        DateOnly attendanceDate,
        CancellationToken cancellationToken = default)
    {
        var exists = await _assignmentRepository.AnyAsync(
            x =>
                x.EmployeeId == employeeId &&
                x.IsActive &&
                !x.IsDeleted &&
                x.EffectiveFrom <= attendanceDate &&
                (!x.EffectiveTo.HasValue ||
                 x.EffectiveTo.Value >= attendanceDate),
            cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(
                "Employee shift assignment",
                employeeId);
        }
    }

    public async Task EnsureAttendanceRecordDoesNotExistAsync(
        Guid employeeId,
        DateOnly attendanceDate,
        CancellationToken cancellationToken = default)
    {
        var exists = await _attendanceRecordRepository.AnyAsync(
            x =>
                x.EmployeeId == employeeId &&
                x.AttendanceDate == attendanceDate &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Attendance record already exists for employee {employeeId} on {attendanceDate}.");
        }
    }
}
