using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.DeleteAttendanceSource;

public sealed class DeleteAttendanceSourceCommandHandler
    : IRequestHandler<DeleteAttendanceSourceCommand>
{
    private readonly IReadRepository<AttendanceSource, Guid>
        _readRepository;

    private readonly IWriteRepository<AttendanceSource, Guid>
        _writeRepository;

    public DeleteAttendanceSourceCommandHandler(
        IReadRepository<AttendanceSource, Guid> readRepository,
        IWriteRepository<AttendanceSource, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteAttendanceSourceCommand request,
        CancellationToken cancellationToken)
    {
        var source = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (source is null || source.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Source",
                request.Id);
        }

        await _writeRepository.DeleteAsync(
            source,
            cancellationToken);
    }
}
