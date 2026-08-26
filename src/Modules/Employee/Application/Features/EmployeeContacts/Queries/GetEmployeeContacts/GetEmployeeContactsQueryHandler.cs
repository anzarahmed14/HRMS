using HRMS.Application.Features.EmployeeContacts.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Queries.GetEmployeeContacts;

public sealed class GetEmployeeContactsQueryHandler
    : IRequestHandler<
        GetEmployeeContactsQuery,
        PagedResult<EmployeeContactDto>>
{
    private readonly IReadRepository<EmployeeContact, Guid> _repository;

    public GetEmployeeContactsQueryHandler(
        IReadRepository<EmployeeContact, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeContactDto>> Handle(
        GetEmployeeContactsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeContactDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeContactDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    ContactType = x.ContactType,
                    ContactValue = x.ContactValue,
                    IsPrimary = x.IsPrimary
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
