using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceSources.BusinessRules;

public class AttendanceSourceBusinessRules
{
    private readonly IReadRepository<AttendanceSource, Guid>
        _repository;

    public AttendanceSourceBusinessRules(
        IReadRepository<AttendanceSource, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureCodeUniqueAsync(
        Guid companyId,
        string code,
        Guid? sourceId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                !x.IsDeleted &&
                (!sourceId.HasValue || x.Id != sourceId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance source code already exists for this company.");
        }
    }
}
