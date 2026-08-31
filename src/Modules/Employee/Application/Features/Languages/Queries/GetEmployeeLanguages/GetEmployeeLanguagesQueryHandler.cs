using HRMS.Application.Features.Languages.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Languages.Queries.GetEmployeeLanguages;

public sealed class GetEmployeeLanguagesQueryHandler
    : IRequestHandler<
        GetEmployeeLanguagesQuery,
        PagedResult<EmployeeLanguageDto>>
{
    private readonly IReadRepository<EmployeeLanguage, Guid> _repository;

    public GetEmployeeLanguagesQueryHandler(
        IReadRepository<EmployeeLanguage, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeLanguageDto>> Handle(
        GetEmployeeLanguagesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeLanguageDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeLanguageDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    LanguageId = x.LanguageId,
                    ProficiencyLevel = x.ProficiencyLevel,
                    CanRead = x.CanRead,
                    CanWrite = x.CanWrite,
                    CanSpeak = x.CanSpeak,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
