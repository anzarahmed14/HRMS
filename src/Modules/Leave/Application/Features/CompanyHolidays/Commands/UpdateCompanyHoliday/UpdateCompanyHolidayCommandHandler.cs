using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed class UpdateCompanyHolidayCommandHandler
    : IRequestHandler<UpdateCompanyHolidayCommand>
{
    private readonly IReadRepository<CompanyHoliday, Guid> _readRepository;
    private readonly IWriteRepository<CompanyHoliday, Guid> _writeRepository;
    private readonly CompanyHolidayBusinessRules _businessRules;

    public UpdateCompanyHolidayCommandHandler(
        IReadRepository<CompanyHoliday, Guid> readRepository,
        IWriteRepository<CompanyHoliday, Guid> writeRepository,
        CompanyHolidayBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateCompanyHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var holiday = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (holiday is null || holiday.IsDeleted)
        {
            throw new NotFoundException(
                "Company Holiday",
                request.Id);
        }

        var leaveYear =
            await _businessRules.EnsureLeaveYearIsValidAsync(
                request.LeaveYearId,
                cancellationToken);

        _businessRules.EnsureHolidayDateIsWithinLeaveYear(
            request.HolidayDate,
            leaveYear);

        var duplicate = await _readRepository.AnyAsync(
            x =>
                x.Id != request.Id &&
                x.LeaveYearId == request.LeaveYearId &&
                x.HolidayDate == request.HolidayDate &&
                !x.IsDeleted,
            cancellationToken);

        if (duplicate)
        {
            throw new ConflictException(
                "A holiday already exists for this date in the selected leave year.");
        }

        holiday.LeaveYearId = request.LeaveYearId;
        holiday.HolidayDate = request.HolidayDate;
        holiday.Name = request.Name.Trim();
        holiday.HolidayType = request.HolidayType.Trim();
        holiday.IsOptional = request.IsOptional;
        holiday.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            holiday,
            cancellationToken);
    }
}
