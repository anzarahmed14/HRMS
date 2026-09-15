using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatusById;

public class GetLeaveYearStatusByIdQueryHandler
    : IRequestHandler<GetLeaveYearStatusByIdQuery, LeaveYearStatusDto>
{
    private readonly IReadRepository<LeaveYearStatus, Guid>
        _repository;

    public GetLeaveYearStatusByIdQueryHandler(
        IReadRepository<LeaveYearStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeaveYearStatusDto> Handle(
        GetLeaveYearStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Year Status",
                request.Id);
        }

        return new LeaveYearStatusDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }
}
