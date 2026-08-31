using HRMS.Application.Features.Nominees.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Nominees.Queries.GetEmployeeNominees;

public sealed class GetEmployeeNomineesQueryHandler
    : IRequestHandler<
        GetEmployeeNomineesQuery,
        PagedResult<EmployeeNomineeDto>>
{
    private readonly IReadRepository<EmployeeNominee, Guid> _repository;

    public GetEmployeeNomineesQueryHandler(
        IReadRepository<EmployeeNominee, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeNomineeDto>> Handle(
        GetEmployeeNomineesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeNomineeDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeNomineeDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    RelationshipId = x.RelationshipId,
                    DateOfBirth = x.DateOfBirth,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    IsMinor = x.IsMinor,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
