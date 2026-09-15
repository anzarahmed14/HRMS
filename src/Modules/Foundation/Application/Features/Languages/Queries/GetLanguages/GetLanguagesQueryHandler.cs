using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Languages.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Queries.GetLanguages;

public class GetLanguagesQueryHandler
    : IRequestHandler<
        GetLanguagesQuery,
        PagedResult<LanguageDto>>
{
    private readonly IReadRepository<Language, Guid> _repository;

    public GetLanguagesQueryHandler(
        IReadRepository<Language, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LanguageDto>> Handle(
        GetLanguagesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LanguageDto>
        {
            Items = result.Items
                .Select(x => new LanguageDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
