using HRMS.Application.Features.EmployeeAddresses.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Queries.GetEmployeeAddresses;

public sealed class GetEmployeeAddressesQueryHandler
    : IRequestHandler<
        GetEmployeeAddressesQuery,
        PagedResult<EmployeeAddressDto>>
{
    private readonly IReadRepository<EmployeeAddress, Guid> _repository;

    public GetEmployeeAddressesQueryHandler(
        IReadRepository<EmployeeAddress, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeAddressDto>> Handle(
        GetEmployeeAddressesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeAddressDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeAddressDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    AddressTypeId = x.AddressTypeId,
                    CountryId = x.CountryId,
                    StateId = x.StateId,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    City = x.City,
                    PostalCode = x.PostalCode,
                    IsPrimary = x.IsPrimary
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
