using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.UpdateAttendanceRegularization;

public sealed class UpdateAttendanceRegularizationCommandHandler
    : IRequestHandler<UpdateAttendanceRegularizationCommand>
{
    private static readonly Guid PendingStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000101");

    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IWriteRepository<AttendanceRegularization, Guid>
        _writeRepository;

    public UpdateAttendanceRegularizationCommandHandler(
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IWriteRepository<AttendanceRegularization, Guid> writeRepository)
    {
        _regularizationRepository = regularizationRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        UpdateAttendanceRegularizationCommand request,
        CancellationToken cancellationToken)
    {
        var regularization =
            await _regularizationRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (regularization is null ||
            regularization.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance regularization",
                request.Id);
        }

        if (regularization.AttendanceRegularizationStatusId !=
            PendingStatusId)
        {
            throw new ConflictException(
                "Only pending attendance regularizations can be updated.");
        }

        regularization.RequestedCheckIn =
            request.RequestedCheckIn;

        regularization.RequestedCheckOut =
            request.RequestedCheckOut;

        regularization.Reason =
            request.Reason;

        await _writeRepository.UpdateAsync(
            regularization,
            cancellationToken);
    }
}
