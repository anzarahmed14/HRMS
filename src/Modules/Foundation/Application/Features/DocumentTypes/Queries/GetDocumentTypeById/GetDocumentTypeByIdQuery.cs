using HRMS.Modules.Foundation.Application.Features.DocumentTypes.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Queries.GetDocumentTypeById;

public record GetDocumentTypeByIdQuery(
    Guid Id) : IRequest<DocumentTypeDto>;
