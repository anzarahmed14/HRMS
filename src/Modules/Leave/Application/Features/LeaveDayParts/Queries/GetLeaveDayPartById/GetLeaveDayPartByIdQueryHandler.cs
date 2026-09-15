using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Queries.GetLeaveDayPartById;

public class GetLeaveDayPartByIdQueryHandler
    : IRequestHandler<GetLeaveDayPartByIdQuery, LeaveDayPartDto>
{
    private readonly IReadRepository<LeaveDayPart, Guid> _repository;

    public GetLeaveDayPartByIdQueryHandler(
        IReadRepository<LeaveDayPart, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeaveDayPartDto> Handle(
        GetLeaveDayPartByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "LeaveDayPart",
                request.Id);
        }

        return new LeaveDayPartDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            DaysValue = entity.DaysValue,
            IsActive = entity.IsActive
        };
    }
}
