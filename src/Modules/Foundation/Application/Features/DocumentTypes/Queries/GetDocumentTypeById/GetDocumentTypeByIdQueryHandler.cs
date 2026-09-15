using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Queries.GetDocumentTypeById;

public class GetDocumentTypeByIdQueryHandler
    : IRequestHandler<GetDocumentTypeByIdQuery, DocumentTypeDto>
{
    private readonly IReadRepository<DocumentType, Guid> _repository;

    public GetDocumentTypeByIdQueryHandler(
        IReadRepository<DocumentType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<DocumentTypeDto> Handle(
        GetDocumentTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "DocumentType",
                request.Id);
        }

        return new DocumentTypeDto
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
