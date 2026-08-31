using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Employee.Domain.Entities;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Application.Features.Documents.BusinessRules;

public class EmployeeDocumentBusinessRules
{
    private readonly IReadRepository<Employee, Guid> _employeeRepository;
    private readonly IReadRepository<EmployeeDocument, Guid> _documentRepository;
    private readonly IReadRepository<DocumentType, Guid> _documentTypeRepository;

    public EmployeeDocumentBusinessRules(
        IReadRepository<Employee, Guid> employeeRepository,
        IReadRepository<EmployeeDocument, Guid> documentRepository,
        IReadRepository<DocumentType, Guid> documentTypeRepository)
    {
        _employeeRepository = employeeRepository;
        _documentRepository = documentRepository;
        _documentTypeRepository = documentTypeRepository;
    }

    public async Task EnsureEmployeeExistsAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException("Employee", employeeId);
        }
    }

    public async Task EnsureDocumentTypeExistsAsync(
        Guid documentTypeId,
        CancellationToken cancellationToken = default)
    {
        var documentType = await _documentTypeRepository.GetByIdAsync(
            documentTypeId,
            cancellationToken);

        if (documentType is null)
        {
            throw new NotFoundException("DocumentType", documentTypeId);
        }
    }

    public async Task EnsureStorageKeyAvailableAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        var exists = await _documentRepository.AnyAsync(
            x => x.StorageKey == storageKey && !x.IsDeleted,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "A document already exists with this storage key.");
        }
    }
}
