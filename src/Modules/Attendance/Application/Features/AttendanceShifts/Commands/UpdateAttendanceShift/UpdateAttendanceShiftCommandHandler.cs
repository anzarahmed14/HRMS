using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.UpdateAttendanceShift;

public sealed class UpdateAttendanceShiftCommandHandler
    : IRequestHandler<UpdateAttendanceShiftCommand>
{
    private readonly IReadRepository<AttendanceShift, Guid> _readRepository;
    private readonly IWriteRepository<AttendanceShift, Guid> _writeRepository;
    private readonly AttendanceShiftBusinessRules _businessRules;

    public UpdateAttendanceShiftCommandHandler(
        IReadRepository<AttendanceShift, Guid> readRepository,
        IWriteRepository<AttendanceShift, Guid> writeRepository,
        AttendanceShiftBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateAttendanceShiftCommand request,
        CancellationToken cancellationToken)
    {
        var shift = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (shift is null || shift.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Shift",
                request.Id);
        }

        await _businessRules.EnsureShiftCodeUniqueAsync(
            shift.CompanyId,
            request.Code,
            request.Id,
            cancellationToken);

        shift.Code = request.Code.Trim();
        shift.Name = request.Name.Trim();
        shift.Description = request.Description?.Trim() ?? string.Empty;
        shift.StartTime = request.StartTime;
        shift.EndTime = request.EndTime;
        shift.BreakMinutes = request.BreakMinutes;
        shift.IsOvernight = request.IsOvernight;
        shift.IsActive = request.IsActive;
        shift.EffectiveFrom = request.EffectiveFrom;
        shift.EffectiveTo = request.EffectiveTo;

        await _writeRepository.UpdateAsync(
            shift,
            cancellationToken);
    }
}
