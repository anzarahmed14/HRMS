using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecordById;

public sealed class GetAttendanceRecordByIdQueryHandler
    : IRequestHandler<GetAttendanceRecordByIdQuery, AttendanceRecordDto>
{
    private readonly IReadRepository<AttendanceRecord, Guid>
        _repository;

    public GetAttendanceRecordByIdQueryHandler(
        IReadRepository<AttendanceRecord, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AttendanceRecordDto> Handle(
        GetAttendanceRecordByIdQuery request,
        CancellationToken cancellationToken)
    {
        var record = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (record is null || record.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance record",
                request.Id);
        }

        return new AttendanceRecordDto
        {
            Id = record.Id,
            EmployeeId = record.EmployeeId,
            AttendanceShiftId = record.AttendanceShiftId,
            AttendancePolicyId = record.AttendancePolicyId,
            AttendanceDate = record.AttendanceDate,
            CheckIn = record.CheckIn,
            CheckOut = record.CheckOut,
            WorkedMinutes = record.WorkedMinutes,
            LateMinutes = record.LateMinutes,
            EarlyLeaveMinutes = record.EarlyLeaveMinutes,
            OvertimeMinutes = record.OvertimeMinutes,
            Status = record.Status,
            Remarks = record.Remarks
        };
    }
}
