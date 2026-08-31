using HRMS.Application.Features.Documents.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Documents.Queries.GetEmployeeDocuments;

public sealed class GetEmployeeDocumentsQueryHandler
    : IRequestHandler<
        GetEmployeeDocumentsQuery,
        PagedResult<EmployeeDocumentDto>>
{
    private readonly IReadRepository<EmployeeDocument, Guid> _repository;

    public GetEmployeeDocumentsQueryHandler(
        IReadRepository<EmployeeDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeDocumentDto>> Handle(
        GetEmployeeDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeDocumentDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeDocumentDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    UploadedOn = x.UploadedOn,
                    IsVerified = x.IsVerified,
                    VerifiedOn = x.VerifiedOn,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
