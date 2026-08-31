using AutoMapper;
using HRMS.Application.Features.Documents.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Documents.Commands.UpdateEmployeeDocument;

public class UpdateEmployeeDocumentCommandHandler
    : IRequestHandler<UpdateEmployeeDocumentCommand>
{
    private readonly IReadRepository<EmployeeDocument, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeDocument, Guid> _writeRepository;
    private readonly EmployeeDocumentBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeDocumentCommandHandler(
        IReadRepository<EmployeeDocument, Guid> readRepository,
        IWriteRepository<EmployeeDocument, Guid> writeRepository,
        EmployeeDocumentBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (document is null)
        {
            throw new InvalidOperationException(
                "Employee document could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureDocumentTypeExistsAsync(
            request.DocumentTypeId,
            cancellationToken);

        await _businessRules.EnsureStorageKeyAvailableAsync(
            request.StorageKey,
            cancellationToken);

        _mapper.Map(request, document);

        await _writeRepository.UpdateAsync(
            document,
            cancellationToken);
    }
}
