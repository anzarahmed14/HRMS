using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypeById;

public class GetIdentifierTypeByIdQueryHandler
    : IRequestHandler<GetIdentifierTypeByIdQuery, IdentifierTypeDto>
{
    private readonly IReadRepository<IdentifierType, Guid> _repository;

    public GetIdentifierTypeByIdQueryHandler(
        IReadRepository<IdentifierType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IdentifierTypeDto> Handle(
        GetIdentifierTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "IdentifierType",
                request.Id);
        }

        return new IdentifierTypeDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsSensitive = entity.IsSensitive,
            IsActive = entity.IsActive
        };
    }
}
