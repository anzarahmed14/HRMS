using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Queries.GetDocumentTypes;

public class GetDocumentTypesQueryHandler
    : IRequestHandler<
        GetDocumentTypesQuery,
        PagedResult<DocumentTypeDto>>
{
    private readonly IReadRepository<DocumentType, Guid> _repository;

    public GetDocumentTypesQueryHandler(
        IReadRepository<DocumentType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<DocumentTypeDto>> Handle(
        GetDocumentTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<DocumentTypeDto>
        {
            Items = result.Items
                .Select(x => new DocumentTypeDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    IsSensitive = x.IsSensitive,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
