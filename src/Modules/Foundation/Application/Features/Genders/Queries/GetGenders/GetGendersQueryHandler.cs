using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Genders.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Queries.GetGenders;

public class GetGendersQueryHandler
    : IRequestHandler<
        GetGendersQuery,
        PagedResult<GenderDto>>
{
    private readonly IReadRepository<Gender, Guid> _repository;

    public GetGendersQueryHandler(
        IReadRepository<Gender, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<GenderDto>> Handle(
        GetGendersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<GenderDto>
        {
            Items = result.Items
                .Select(x => new GenderDto
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
