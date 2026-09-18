using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.BusinessRules;

public class AttendanceRegularizationStatusBusinessRules
{
    private readonly IReadRepository<AttendanceRegularizationStatus, Guid>
        _repository;

    public AttendanceRegularizationStatusBusinessRules(
        IReadRepository<AttendanceRegularizationStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid? statusId = null,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x =>
                x.Code == code &&
                !x.IsDeleted &&
                (!statusId.HasValue || x.Id != statusId.Value),
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance regularization status code already exists.");
        }
    }

    public async Task<AttendanceRegularizationStatus> EnsureExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var status = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (status is null || status.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Regularization Status",
                id);
        }

        return status;
    }
}
