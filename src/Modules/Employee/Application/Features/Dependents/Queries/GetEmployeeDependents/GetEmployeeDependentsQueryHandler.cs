using HRMS.Application.Features.Dependents.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Dependents.Queries.GetEmployeeDependents;

public sealed class GetEmployeeDependentsQueryHandler
    : IRequestHandler<
        GetEmployeeDependentsQuery,
        PagedResult<EmployeeDependentDto>>
{
    private readonly IReadRepository<EmployeeDependent, Guid> _repository;

    public GetEmployeeDependentsQueryHandler(
        IReadRepository<EmployeeDependent, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeDependentDto>> Handle(
        GetEmployeeDependentsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeDependentDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeDependentDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    RelationshipId = x.RelationshipId,
                    GenderId = x.GenderId,
                    DateOfBirth = x.DateOfBirth,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    IsDependent = x.IsDependent,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
