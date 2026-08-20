using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;

public sealed class GetCompanyHolidayByIdQueryHandler
    : IRequestHandler<GetCompanyHolidayByIdQuery, CompanyHolidayDto>
{
    private readonly IReadRepository<CompanyHoliday, Guid> _repository;

    public GetCompanyHolidayByIdQueryHandler(
        IReadRepository<CompanyHoliday, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<CompanyHolidayDto> Handle(
        GetCompanyHolidayByIdQuery request,
        CancellationToken cancellationToken)
    {
        var holiday = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (holiday is null || holiday.IsDeleted)
        {
            throw new NotFoundException(
                "Company Holiday",
                request.Id);
        }

        return new CompanyHolidayDto
        {
            Id = holiday.Id,
            LeaveYearId = holiday.LeaveYearId,
            HolidayDate = holiday.HolidayDate,
            Name = holiday.Name,
            HolidayType = holiday.HolidayType,
            IsOptional = holiday.IsOptional,
            IsActive = holiday.IsActive
        };
    }
}
