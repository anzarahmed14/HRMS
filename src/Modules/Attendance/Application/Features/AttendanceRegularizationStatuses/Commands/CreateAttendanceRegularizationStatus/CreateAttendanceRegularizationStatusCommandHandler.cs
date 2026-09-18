using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.CreateAttendanceRegularizationStatus;

public sealed class CreateAttendanceRegularizationStatusCommandHandler
    : IRequestHandler<CreateAttendanceRegularizationStatusCommand, Guid>
{
    private readonly IWriteRepository<AttendanceRegularizationStatus, Guid>
        _writeRepository;

    private readonly AttendanceRegularizationStatusBusinessRules
        _businessRules;

    public CreateAttendanceRegularizationStatusCommandHandler(
        IWriteRepository<AttendanceRegularizationStatus, Guid> writeRepository,
        AttendanceRegularizationStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAttendanceRegularizationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken: cancellationToken);

        var status = new AttendanceRegularizationStatus
        {
            Code = code,
            Name = request.Name.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            status,
            cancellationToken);

        return status.Id;
    }
}
