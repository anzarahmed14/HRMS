using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypes;

public sealed class GetAddressTypesQueryHandler
    : IRequestHandler<GetAddressTypesQuery, PagedResult<AddressTypeDto>>
{
    private readonly IReadRepository<AddressType, Guid> _repository;

    public GetAddressTypesQueryHandler(
        IReadRepository<AddressType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AddressTypeDto>> Handle(
        GetAddressTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<AddressTypeDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new AddressTypeDto
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
