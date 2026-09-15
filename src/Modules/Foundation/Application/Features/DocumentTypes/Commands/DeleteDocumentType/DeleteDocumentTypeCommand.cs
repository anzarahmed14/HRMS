using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.DeleteDocumentType;

public record DeleteDocumentTypeCommand(
    Guid Id) : IRequest;
