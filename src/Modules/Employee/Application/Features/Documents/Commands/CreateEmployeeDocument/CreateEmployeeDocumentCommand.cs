using MediatR;

namespace HRMS.Application.Features.Documents.Commands.CreateEmployeeDocument;

public record CreateEmployeeDocumentCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public Guid DocumentTypeId { get; init; }

    public string DocumentName { get; init; } = string.Empty;

    public string FileName { get; init; } = string.Empty;

    public string StorageKey { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public long FileSize { get; init; }
}
