using HRMS.Application.Features.EmploymentTypes.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Queries.GetEmploymentTypes;

public sealed class GetEmploymentTypesQueryHandler
    : IRequestHandler<
        GetEmploymentTypesQuery,
        PagedResult<EmploymentTypeDto>>
{
    private readonly IReadRepository<EmploymentType, Guid> _repository;

    public GetEmploymentTypesQueryHandler(
        IReadRepository<EmploymentType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmploymentTypeDto>> Handle(
        GetEmploymentTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<EmploymentTypeDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmploymentTypeDto
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
