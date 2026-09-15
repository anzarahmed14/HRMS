using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.UpdateDocumentType;

public class UpdateDocumentTypeCommandHandler
    : IRequestHandler<UpdateDocumentTypeCommand>
{
    private readonly IReadRepository<DocumentType, Guid> _readRepository;
    private readonly IWriteRepository<DocumentType, Guid> _writeRepository;
    private readonly DocumentTypeBusinessRules _businessRules;

    public UpdateDocumentTypeCommandHandler(
        IReadRepository<DocumentType, Guid> readRepository,
        IWriteRepository<DocumentType, Guid> writeRepository,
        DocumentTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateDocumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            await _businessRules.EnsureDocumentTypeExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            request.Id,
            cancellationToken);

        entity.Code = code;
        entity.Name = name;
        entity.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        entity.IsSensitive = request.IsSensitive;
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
