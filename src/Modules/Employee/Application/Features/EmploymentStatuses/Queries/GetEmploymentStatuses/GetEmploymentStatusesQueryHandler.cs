using HRMS.Application.Features.EmploymentStatuses.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Queries.GetEmploymentStatuses;

public sealed class GetEmploymentStatusesQueryHandler
    : IRequestHandler<
        GetEmploymentStatusesQuery,
        PagedResult<EmploymentStatusDto>>
{
    private readonly IReadRepository<EmploymentStatus, Guid> _repository;

    public GetEmploymentStatusesQueryHandler(
        IReadRepository<EmploymentStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmploymentStatusDto>> Handle(
        GetEmploymentStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<EmploymentStatusDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmploymentStatusDto
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
