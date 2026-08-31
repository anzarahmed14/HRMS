using HRMS.Application.Features.Documents.DTOs;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;

namespace HRMS.Application.Features.Documents.Queries.GetEmployeeDocuments;

public sealed record GetEmployeeDocumentsQuery(
    Guid EmployeeId,
    PagedRequest Request
) : IRequest<PagedResult<EmployeeDocumentDto>>;
