using HRMS.Application.Features.Documents.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Documents.Commands.CreateEmployeeDocument;

public class CreateEmployeeDocumentCommandHandler
    : IRequestHandler<CreateEmployeeDocumentCommand, Guid>
{
    private readonly IWriteRepository<EmployeeDocument, Guid> _writeRepository;
    private readonly EmployeeDocumentBusinessRules _businessRules;

    public CreateEmployeeDocumentCommandHandler(
        IWriteRepository<EmployeeDocument, Guid> writeRepository,
        EmployeeDocumentBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeDocumentCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureDocumentTypeExistsAsync(
            request.DocumentTypeId,
            cancellationToken);

        await _businessRules.EnsureStorageKeyAvailableAsync(
            request.StorageKey,
            cancellationToken);

        var document = new EmployeeDocument
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            DocumentTypeId = request.DocumentTypeId,
            DocumentName = request.DocumentName,
            FileName = request.FileName,
            StorageKey = request.StorageKey,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            UploadedOn = DateTimeOffset.UtcNow,
            IsVerified = false,
            VerifiedOn = null,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            document,
            cancellationToken);

        return document.Id;
    }
}
