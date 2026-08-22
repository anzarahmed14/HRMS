using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.UpdateAttendanceSource;

public sealed class UpdateAttendanceSourceCommandHandler
    : IRequestHandler<UpdateAttendanceSourceCommand>
{
    private readonly IReadRepository<AttendanceSource, Guid>
        _readRepository;

    private readonly IWriteRepository<AttendanceSource, Guid>
        _writeRepository;

    private readonly AttendanceSourceBusinessRules
        _businessRules;

    public UpdateAttendanceSourceCommandHandler(
        IReadRepository<AttendanceSource, Guid> readRepository,
        IWriteRepository<AttendanceSource, Guid> writeRepository,
        AttendanceSourceBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateAttendanceSourceCommand request,
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

        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            source.CompanyId,
            code,
            request.Id,
            cancellationToken);

        source.Code = code;
        source.Name = request.Name.Trim();
        source.SourceType = request.SourceType.Trim();
        source.Description = request.Description?.Trim();
        source.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            source,
            cancellationToken);
    }
}
