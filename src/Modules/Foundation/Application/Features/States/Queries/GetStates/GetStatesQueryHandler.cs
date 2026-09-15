using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.States.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Queries.GetStates;

public sealed class GetStatesQueryHandler
    : IRequestHandler<GetStatesQuery, PagedResult<StateDto>>
{
    private readonly IReadRepository<State, Guid> _repository;

    public GetStatesQueryHandler(
        IReadRepository<State, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<StateDto>> Handle(
        GetStatesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.CountryId == request.CountryId,
            cancellationToken: cancellationToken);

        return new PagedResult<StateDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new StateDto
                {
                    Id = x.Id,
                    CountryId = x.CountryId,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
