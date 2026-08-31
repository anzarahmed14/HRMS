using HRMS.Application.Features.Languages.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Languages.Queries.GetEmployeeLanguageById;

public class GetEmployeeLanguageByIdQueryHandler
    : IRequestHandler<GetEmployeeLanguageByIdQuery, EmployeeLanguageDto?>
{
    private readonly IReadRepository<EmployeeLanguage, Guid> _repository;

    public GetEmployeeLanguageByIdQueryHandler(
        IReadRepository<EmployeeLanguage, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeLanguageDto?> Handle(
        GetEmployeeLanguageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new EmployeeLanguageDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            LanguageId = entity.LanguageId,
            ProficiencyLevel = entity.ProficiencyLevel,
            CanRead = entity.CanRead,
            CanWrite = entity.CanWrite,
            CanSpeak = entity.CanSpeak,
            IsActive = entity.IsActive
        };
    }
}
