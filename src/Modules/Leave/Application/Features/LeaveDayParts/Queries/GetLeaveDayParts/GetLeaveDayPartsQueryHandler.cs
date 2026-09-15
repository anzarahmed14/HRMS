using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Queries.GetLeaveDayParts;

public class GetLeaveDayPartsQueryHandler
    : IRequestHandler<
        GetLeaveDayPartsQuery,
        PagedResult<LeaveDayPartDto>>
{
    private readonly IReadRepository<LeaveDayPart, Guid> _repository;

    public GetLeaveDayPartsQueryHandler(
        IReadRepository<LeaveDayPart, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeaveDayPartDto>> Handle(
        GetLeaveDayPartsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeaveDayPartDto>
        {
            Items = result.Items
                .Select(x => new LeaveDayPartDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    DaysValue = x.DaysValue,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
