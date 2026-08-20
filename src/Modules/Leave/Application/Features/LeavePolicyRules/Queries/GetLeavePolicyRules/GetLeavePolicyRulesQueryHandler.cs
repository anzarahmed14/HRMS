using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Queries.GetLeavePolicyRules;

public class GetLeavePolicyRulesQueryHandler
    : IRequestHandler<GetLeavePolicyRulesQuery, PagedResult<LeavePolicyRuleListDto>>
{
    private readonly IReadRepository<LeavePolicyRule, Guid> _repository;

    public GetLeavePolicyRulesQueryHandler(
        IReadRepository<LeavePolicyRule, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeavePolicyRuleListDto>> Handle(
        GetLeavePolicyRulesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeavePolicyRuleListDto>
        {
            Items = result.Items
                .Select(x => new LeavePolicyRuleListDto
                {
                    Id = x.Id,
                    LeavePolicyId = x.LeavePolicyId,
                    LeaveTypeId = x.LeaveTypeId,
                    AnnualEntitlement = x.AnnualEntitlement
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
