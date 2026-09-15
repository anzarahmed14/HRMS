using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.Relationships.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Queries.GetRelationshipById;

public class GetRelationshipByIdQueryHandler
    : IRequestHandler<GetRelationshipByIdQuery, RelationshipDto>
{
    private readonly IReadRepository<Relationship, Guid> _repository;

    public GetRelationshipByIdQueryHandler(
        IReadRepository<Relationship, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<RelationshipDto> Handle(
        GetRelationshipByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Relationship",
                request.Id);
        }

        return new RelationshipDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}
