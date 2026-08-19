using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies.Queries.GetLeavePolicyById;

public class GetLeavePolicyByIdQueryHandler
    : IRequestHandler<GetLeavePolicyByIdQuery, LeavePolicyDto>
{
    private readonly IReadRepository<LeavePolicy, Guid> _repository;

    public GetLeavePolicyByIdQueryHandler(
        IReadRepository<LeavePolicy, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeavePolicyDto> Handle(
        GetLeavePolicyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Policy",
                request.Id);
        }

        return new LeavePolicyDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}
