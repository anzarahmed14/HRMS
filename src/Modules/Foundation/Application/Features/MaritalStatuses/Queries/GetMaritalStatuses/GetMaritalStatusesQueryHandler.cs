using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Queries.GetMaritalStatuses;

public class GetMaritalStatusesQueryHandler
    : IRequestHandler<
        GetMaritalStatusesQuery,
        PagedResult<MaritalStatusDto>>
{
    private readonly IReadRepository<MaritalStatus, Guid> _repository;

    public GetMaritalStatusesQueryHandler(
        IReadRepository<MaritalStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<MaritalStatusDto>> Handle(
        GetMaritalStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<MaritalStatusDto>
        {
            Items = result.Items
                .Select(x => new MaritalStatusDto
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
