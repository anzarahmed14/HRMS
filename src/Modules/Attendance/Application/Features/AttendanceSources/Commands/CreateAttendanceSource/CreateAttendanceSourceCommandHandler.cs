using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.CreateAttendanceSource;

public sealed class CreateAttendanceSourceCommandHandler
    : IRequestHandler<CreateAttendanceSourceCommand, Guid>
{
    private readonly IWriteRepository<AttendanceSource, Guid>
        _writeRepository;

    private readonly AttendanceSourceBusinessRules
        _businessRules;

    public CreateAttendanceSourceCommandHandler(
        IWriteRepository<AttendanceSource, Guid> writeRepository,
        AttendanceSourceBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAttendanceSourceCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            request.CompanyId,
            code,
            cancellationToken: cancellationToken);

        var source = new AttendanceSource
        {
            CompanyId = request.CompanyId,
            Code = code,
            Name = request.Name.Trim(),
            SourceType = request.SourceType.Trim(),
            Description = request.Description?.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            source,
            cancellationToken);

        return source.Id;
    }
}
