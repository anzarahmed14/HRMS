using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Relationships.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Queries.GetRelationships;

public class GetRelationshipsQueryHandler
    : IRequestHandler<GetRelationshipsQuery, PagedResult<RelationshipDto>>
{
    private readonly IReadRepository<Relationship, Guid> _repository;

    public GetRelationshipsQueryHandler(
        IReadRepository<Relationship, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<RelationshipDto>> Handle(
        GetRelationshipsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<RelationshipDto>
        {
            Items = result.Items
                .Select(x => new RelationshipDto
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
