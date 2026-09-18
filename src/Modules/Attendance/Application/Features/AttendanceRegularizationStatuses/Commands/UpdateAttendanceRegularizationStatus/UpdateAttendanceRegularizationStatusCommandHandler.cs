using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.UpdateAttendanceRegularizationStatus;

public sealed class UpdateAttendanceRegularizationStatusCommandHandler
    : IRequestHandler<UpdateAttendanceRegularizationStatusCommand>
{
    private readonly IWriteRepository<AttendanceRegularizationStatus, Guid>
        _writeRepository;

    private readonly AttendanceRegularizationStatusBusinessRules
        _businessRules;

    public UpdateAttendanceRegularizationStatusCommandHandler(
        IWriteRepository<AttendanceRegularizationStatus, Guid> writeRepository,
        AttendanceRegularizationStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateAttendanceRegularizationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var status = await _businessRules.EnsureExistsAsync(
            request.Id,
            cancellationToken);

        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        status.Code = code;
        status.Name = request.Name.Trim();
        status.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            status,
            cancellationToken);
    }
}
