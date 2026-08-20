using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.BusinessRules;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed class CreateCompanyHolidayCommandHandler
    : IRequestHandler<CreateCompanyHolidayCommand, Guid>
{
    private readonly IWriteRepository<CompanyHoliday, Guid> _writeRepository;
    private readonly CompanyHolidayBusinessRules _businessRules;

    public CreateCompanyHolidayCommandHandler(
        IWriteRepository<CompanyHoliday, Guid> writeRepository,
        CompanyHolidayBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateCompanyHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var leaveYear =
            await _businessRules.EnsureLeaveYearIsValidAsync(
                request.LeaveYearId,
                cancellationToken);

        _businessRules.EnsureHolidayDateIsWithinLeaveYear(
            request.HolidayDate,
            leaveYear);

        await _businessRules.EnsureHolidayDoesNotExistAsync(
            request.LeaveYearId,
            request.HolidayDate,
            cancellationToken);

        var entity = new CompanyHoliday
        {
            LeaveYearId = request.LeaveYearId,
            HolidayDate = request.HolidayDate,
            Name = request.Name.Trim(),
            HolidayType = request.HolidayType.Trim(),
            IsOptional = request.IsOptional,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
