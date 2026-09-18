using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.UpdateAttendanceDayStatus;

public sealed class UpdateAttendanceDayStatusCommandHandler
    : IRequestHandler<UpdateAttendanceDayStatusCommand>
{
    private readonly IWriteRepository<AttendanceDayStatus, Guid>
        _writeRepository;

    private readonly AttendanceDayStatusBusinessRules
        _businessRules;

    public UpdateAttendanceDayStatusCommandHandler(
        IWriteRepository<AttendanceDayStatus, Guid> writeRepository,
        AttendanceDayStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateAttendanceDayStatusCommand request,
        CancellationToken cancellationToken)
    {
        var dayStatus = await _businessRules.EnsureExistsAsync(
            request.Id,
            cancellationToken);

        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        dayStatus.Code = code;
        dayStatus.Name = request.Name.Trim();
        dayStatus.Description = request.Description?.Trim();
        dayStatus.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            dayStatus,
            cancellationToken);
    }
}
