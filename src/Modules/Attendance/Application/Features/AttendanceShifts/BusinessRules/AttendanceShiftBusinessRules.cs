using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using HRMS.Modules.Companies.Domain.Entities;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceShifts.BusinessRules;

public class AttendanceShiftBusinessRules
{
    private readonly IReadRepository<Company, Guid> _companyReadRepository;
    private readonly IReadRepository<AttendanceShift, Guid> _shiftReadRepository;

    public AttendanceShiftBusinessRules(
        IReadRepository<Company, Guid> companyReadRepository,
        IReadRepository<AttendanceShift, Guid> shiftReadRepository)
    {
        _companyReadRepository = companyReadRepository;
        _shiftReadRepository = shiftReadRepository;
    }

    public async Task EnsureCompanyExistsAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await _companyReadRepository.GetByIdAsync(
            companyId,
            cancellationToken);

        if (company is null || company.IsDeleted)
        {
            throw new NotFoundException(
                "Company",
                companyId);
        }
    }

    public async Task EnsureShiftCodeUniqueAsync(
        Guid companyId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var exists = await _shiftReadRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance shift code already exists for this company.");
        }
    }

    public async Task EnsureShiftCodeUniqueAsync(
        Guid companyId,
        string code,
        Guid shiftId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _shiftReadRepository.AnyAsync(
            x =>
                x.CompanyId == companyId &&
                x.Code == code &&
                x.Id != shiftId &&
                !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Attendance shift code already exists for this company.");
        }
    }
}
