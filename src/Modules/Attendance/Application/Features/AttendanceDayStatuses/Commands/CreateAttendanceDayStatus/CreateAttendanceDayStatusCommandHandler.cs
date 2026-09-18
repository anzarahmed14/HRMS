using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.CreateAttendanceDayStatus;

public sealed class CreateAttendanceDayStatusCommandHandler
    : IRequestHandler<CreateAttendanceDayStatusCommand, Guid>
{
    private readonly IWriteRepository<AttendanceDayStatus, Guid>
        _writeRepository;

    private readonly AttendanceDayStatusBusinessRules
        _businessRules;

    public CreateAttendanceDayStatusCommandHandler(
        IWriteRepository<AttendanceDayStatus, Guid> writeRepository,
        AttendanceDayStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAttendanceDayStatusCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken: cancellationToken);

        var dayStatus = new AttendanceDayStatus
        {
            Code = code,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            dayStatus,
            cancellationToken);

        return dayStatus.Id;
    }
}
