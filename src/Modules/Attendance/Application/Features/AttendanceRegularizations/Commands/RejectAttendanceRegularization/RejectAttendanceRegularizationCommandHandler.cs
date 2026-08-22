using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.RejectAttendanceRegularization;

public sealed class RejectAttendanceRegularizationCommandHandler
    : IRequestHandler<RejectAttendanceRegularizationCommand>
{
    private static readonly Guid PendingStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000101");

    private static readonly Guid RejectedStatusId =
        Guid.Parse("10000000-0000-0000-0000-000000000103");

    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IWriteRepository<AttendanceRegularization, Guid>
        _writeRepository;

    private readonly IUserContext _userContext;

    public RejectAttendanceRegularizationCommandHandler(
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IWriteRepository<AttendanceRegularization, Guid> writeRepository,
        IUserContext userContext)
    {
        _regularizationRepository = regularizationRepository;
        _writeRepository = writeRepository;
        _userContext = userContext;
    }

    public async Task Handle(
        RejectAttendanceRegularizationCommand request,
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
                "Only pending attendance regularizations can be rejected.");
        }

        regularization.AttendanceRegularizationStatusId =
            RejectedStatusId;

        regularization.RejectedBy =
            _userContext.UserId;

        regularization.RejectedOn =
            DateTimeOffset.UtcNow;

        regularization.RejectionRemarks =
            request.Remarks;

        await _writeRepository.UpdateAsync(
            regularization,
            cancellationToken);
    }
}
