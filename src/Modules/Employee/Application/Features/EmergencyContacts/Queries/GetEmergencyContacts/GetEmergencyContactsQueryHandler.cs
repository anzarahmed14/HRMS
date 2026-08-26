using HRMS.Application.Features.EmergencyContacts.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Queries.GetEmergencyContacts;

public sealed class GetEmergencyContactsQueryHandler
    : IRequestHandler<
        GetEmergencyContactsQuery,
        PagedResult<EmergencyContactDto>>
{
    private readonly IReadRepository<EmergencyContact, Guid> _repository;

    public GetEmergencyContactsQueryHandler(
        IReadRepository<EmergencyContact, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmergencyContactDto>> Handle(
        GetEmergencyContactsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmergencyContactDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmergencyContactDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    RelationshipId = x.RelationshipId,
                    PhoneNumber = x.PhoneNumber,
                    AlternatePhoneNumber = x.AlternatePhoneNumber,
                    Email = x.Email,
                    IsPrimary = x.IsPrimary
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
