using AutoMapper;
using HRMS.Application.Features.Documents.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Documents.Queries.GetEmployeeDocumentById;

public class GetEmployeeDocumentByIdQueryHandler
    : IRequestHandler<GetEmployeeDocumentByIdQuery, EmployeeDocumentDto?>
{
    private readonly IReadRepository<EmployeeDocument, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeeDocumentByIdQueryHandler(
        IReadRepository<EmployeeDocument, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDocumentDto?> Handle(
        GetEmployeeDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (document is null)
        {
            return null;
        }

        return _mapper.Map<EmployeeDocumentDto>(document);
    }
}
