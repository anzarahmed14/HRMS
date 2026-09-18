using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.BusinessRules;

public class AttendanceDayStatusBusinessRules
{
    private readonly IReadRepository<AttendanceDayStatus, Guid>
        _repository;

    public AttendanceDayStatusBusinessRules(
        IReadRepository<AttendanceDayStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid? dayStatusId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x =>
                x.Code == code &&
                !x.IsDeleted &&
                (!dayStatusId.HasValue || x.Id != dayStatusId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance day status code already exists.");
        }
    }

    public async Task<AttendanceDayStatus> EnsureExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dayStatus = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (dayStatus is null || dayStatus.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Day Status",
                id);
        }

        return dayStatus;
    }
}
