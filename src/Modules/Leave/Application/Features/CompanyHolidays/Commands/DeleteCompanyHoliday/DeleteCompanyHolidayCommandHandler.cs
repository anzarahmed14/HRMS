using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;

public sealed class DeleteCompanyHolidayCommandHandler
    : IRequestHandler<DeleteCompanyHolidayCommand>
{
    private readonly IReadRepository<CompanyHoliday, Guid> _readRepository;
    private readonly IWriteRepository<CompanyHoliday, Guid> _writeRepository;

    public DeleteCompanyHolidayCommandHandler(
        IReadRepository<CompanyHoliday, Guid> readRepository,
        IWriteRepository<CompanyHoliday, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteCompanyHolidayCommand request,
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

        await _writeRepository.DeleteAsync(
            holiday,
            cancellationToken);
    }
}
