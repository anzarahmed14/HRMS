using HRMS.Application.Features.Documents.DTOs;
using MediatR;

namespace HRMS.Application.Features.Documents.Queries.GetEmployeeDocumentById;

public record GetEmployeeDocumentByIdQuery(Guid Id)
    : IRequest<EmployeeDocumentDto?>;
