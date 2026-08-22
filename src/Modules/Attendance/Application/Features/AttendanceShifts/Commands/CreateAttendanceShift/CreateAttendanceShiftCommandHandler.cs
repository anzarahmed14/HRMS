using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.CreateAttendanceShift;

public sealed class CreateAttendanceShiftCommandHandler
    : IRequestHandler<CreateAttendanceShiftCommand, Guid>
{
    private readonly IWriteRepository<AttendanceShift, Guid> _writeRepository;
    private readonly AttendanceShiftBusinessRules _businessRules;

    public CreateAttendanceShiftCommandHandler(
        IWriteRepository<AttendanceShift, Guid> writeRepository,
        AttendanceShiftBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAttendanceShiftCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureCompanyExistsAsync(
            request.CompanyId,
            cancellationToken);

        await _businessRules.EnsureShiftCodeUniqueAsync(
            request.CompanyId,
            request.Code,
            cancellationToken);

        var shift = new AttendanceShift
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            BreakMinutes = request.BreakMinutes,
            IsOvernight = request.IsOvernight,
            IsActive = request.IsActive,
            EffectiveFrom = request.EffectiveFrom,
            EffectiveTo = request.EffectiveTo
        };

        await _writeRepository.AddAsync(
            shift,
            cancellationToken);

        return shift.Id;
    }
}
