using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Queries.GetLeavePolicies;

public class GetLeavePoliciesQueryHandler
    : IRequestHandler<GetLeavePoliciesQuery, PagedResult<LeavePolicyListDto>>
{
    private readonly IReadRepository<LeavePolicy, Guid> _repository;

    public GetLeavePoliciesQueryHandler(
        IReadRepository<LeavePolicy, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeavePolicyListDto>> Handle(
        GetLeavePoliciesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeavePolicyListDto>
        {
            Items = result.Items
                .Select(x => new LeavePolicyListDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
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
