using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Queries.GetLeavePolicyRuleById;

public class GetLeavePolicyRuleByIdQueryHandler
    : IRequestHandler<GetLeavePolicyRuleByIdQuery, LeavePolicyRuleDto>
{
    private readonly IReadRepository<LeavePolicyRule, Guid> _repository;

    public GetLeavePolicyRuleByIdQueryHandler(
        IReadRepository<LeavePolicyRule, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeavePolicyRuleDto> Handle(
        GetLeavePolicyRuleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Policy Rule",
                request.Id);
        }

        return new LeavePolicyRuleDto
        {
            Id = entity.Id,
            LeavePolicyId = entity.LeavePolicyId,
            LeaveTypeId = entity.LeaveTypeId,
            AnnualEntitlement = entity.AnnualEntitlement
        };
    }
}
