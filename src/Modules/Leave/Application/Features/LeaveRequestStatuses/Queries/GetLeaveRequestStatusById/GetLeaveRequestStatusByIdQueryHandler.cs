using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatusById;

public class GetLeaveRequestStatusByIdQueryHandler
    : IRequestHandler<GetLeaveRequestStatusByIdQuery, LeaveRequestStatusDto>
{
    private readonly IReadRepository<LeaveRequestStatus, Guid> _repository;

    public GetLeaveRequestStatusByIdQueryHandler(
        IReadRepository<LeaveRequestStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeaveRequestStatusDto> Handle(
        GetLeaveRequestStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "LeaveRequestStatus",
                request.Id);
        }

        return new LeaveRequestStatusDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }
}
