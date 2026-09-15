using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.CreateDocumentType;

public class CreateDocumentTypeCommandHandler
    : IRequestHandler<CreateDocumentTypeCommand, Guid>
{
    private readonly IWriteRepository<DocumentType, Guid> _writeRepository;
    private readonly DocumentTypeBusinessRules _businessRules;

    public CreateDocumentTypeCommandHandler(
        IWriteRepository<DocumentType, Guid> writeRepository,
        DocumentTypeBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateDocumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            name,
            cancellationToken);

        var entity = new DocumentType
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsSensitive = request.IsSensitive,
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
