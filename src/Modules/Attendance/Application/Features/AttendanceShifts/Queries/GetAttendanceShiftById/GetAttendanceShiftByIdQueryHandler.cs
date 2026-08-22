using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Queries.GetAttendanceShiftById;

public sealed class GetAttendanceShiftByIdQueryHandler
    : IRequestHandler<
        GetAttendanceShiftByIdQuery,
        AttendanceShiftDto>
{
    private readonly IReadRepository<AttendanceShift, Guid> _repository;

    public GetAttendanceShiftByIdQueryHandler(
        IReadRepository<AttendanceShift, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AttendanceShiftDto> Handle(
        GetAttendanceShiftByIdQuery request,
        CancellationToken cancellationToken)
    {
        var shift = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (shift is null || shift.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Shift",
                request.Id);
        }

        return new AttendanceShiftDto
        {
            Id = shift.Id,
            CompanyId = shift.CompanyId,
            Code = shift.Code,
            Name = shift.Name,
            Description = shift.Description,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            BreakMinutes = shift.BreakMinutes,
            IsOvernight = shift.IsOvernight,
            IsActive = shift.IsActive,
            EffectiveFrom = shift.EffectiveFrom,
            EffectiveTo = shift.EffectiveTo
        };
    }
}
