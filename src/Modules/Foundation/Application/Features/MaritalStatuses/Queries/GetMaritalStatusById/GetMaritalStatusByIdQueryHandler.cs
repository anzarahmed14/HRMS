using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Queries.GetMaritalStatusById;

public class GetMaritalStatusByIdQueryHandler
    : IRequestHandler<GetMaritalStatusByIdQuery, MaritalStatusDto>
{
    private readonly IReadRepository<MaritalStatus, Guid> _repository;

    public GetMaritalStatusByIdQueryHandler(
        IReadRepository<MaritalStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<MaritalStatusDto> Handle(
        GetMaritalStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "MaritalStatus",
                request.Id);
        }

        return new MaritalStatusDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}
