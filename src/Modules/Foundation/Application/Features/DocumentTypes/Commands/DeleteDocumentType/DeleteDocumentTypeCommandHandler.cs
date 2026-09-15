using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.DeleteDocumentType;

public class DeleteDocumentTypeCommandHandler
    : IRequestHandler<DeleteDocumentTypeCommand>
{
    private readonly IReadRepository<DocumentType, Guid> _readRepository;
    private readonly IWriteRepository<DocumentType, Guid> _writeRepository;
    private readonly DocumentTypeBusinessRules _businessRules;

    public DeleteDocumentTypeCommandHandler(
        IReadRepository<DocumentType, Guid> readRepository,
        IWriteRepository<DocumentType, Guid> writeRepository,
        DocumentTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteDocumentTypeCommand request,
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

        await _writeRepository.DeleteAsync(
            entity,
            cancellationToken);
    }
}
