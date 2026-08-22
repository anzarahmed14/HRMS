using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.Queries.GetAttendanceSourceById;

public sealed class GetAttendanceSourceByIdQueryHandler
    : IRequestHandler<GetAttendanceSourceByIdQuery, AttendanceSourceDto>
{
    private readonly IReadRepository<AttendanceSource, Guid>
        _repository;

    public GetAttendanceSourceByIdQueryHandler(
        IReadRepository<AttendanceSource, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AttendanceSourceDto> Handle(
        GetAttendanceSourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var source = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (source is null || source.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Source",
                request.Id);
        }

        return new AttendanceSourceDto
        {
            Id = source.Id,
            CompanyId = source.CompanyId,
            Code = source.Code,
            Name = source.Name,
            SourceType = source.SourceType,
            Description = source.Description,
            IsActive = source.IsActive
        };
    }
}
