using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypes;

public class GetIdentifierTypesQueryHandler
    : IRequestHandler<
        GetIdentifierTypesQuery,
        PagedResult<IdentifierTypeDto>>
{
    private readonly IReadRepository<IdentifierType, Guid> _repository;

    public GetIdentifierTypesQueryHandler(
        IReadRepository<IdentifierType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<IdentifierTypeDto>> Handle(
        GetIdentifierTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<IdentifierTypeDto>
        {
            Items = result.Items
                .Select(x => new IdentifierTypeDto
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
