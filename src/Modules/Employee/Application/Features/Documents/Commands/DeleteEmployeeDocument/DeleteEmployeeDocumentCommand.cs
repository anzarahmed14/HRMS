using MediatR;

namespace HRMS.Application.Features.Documents.Commands.DeleteEmployeeDocument;

public record DeleteEmployeeDocumentCommand(Guid Id) : IRequest;
