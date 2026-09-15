using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.DocumentTypes.Queries.GetDocumentTypes;

public record GetDocumentTypesQuery(
    PagedRequest Request) : IRequest<PagedResult<DocumentTypeDto>>;
