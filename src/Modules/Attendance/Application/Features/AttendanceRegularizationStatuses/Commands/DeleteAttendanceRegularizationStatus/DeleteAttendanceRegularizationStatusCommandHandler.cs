using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.DeleteAttendanceRegularizationStatus;

public sealed class DeleteAttendanceRegularizationStatusCommandHandler
    : IRequestHandler<DeleteAttendanceRegularizationStatusCommand>
{
    private readonly IWriteRepository<AttendanceRegularizationStatus, Guid>
        _writeRepository;

    private readonly AttendanceRegularizationStatusBusinessRules
        _businessRules;

    public DeleteAttendanceRegularizationStatusCommandHandler(
        IWriteRepository<AttendanceRegularizationStatus, Guid> writeRepository,
        AttendanceRegularizationStatusBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteAttendanceRegularizationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var status = await _businessRules.EnsureExistsAsync(
            request.Id,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            status,
            cancellationToken);
    }
}
