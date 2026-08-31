using HRMS.Application.Features.Documents.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Documents.Commands.DeleteEmployeeDocument;

public class DeleteEmployeeDocumentCommandHandler
    : IRequestHandler<DeleteEmployeeDocumentCommand>
{
    private readonly IReadRepository<EmployeeDocument, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeDocument, Guid> _writeRepository;
    private readonly EmployeeDocumentBusinessRules _businessRules;

    public DeleteEmployeeDocumentCommandHandler(
        IReadRepository<EmployeeDocument, Guid> readRepository,
        IWriteRepository<EmployeeDocument, Guid> writeRepository,
        EmployeeDocumentBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmployeeDocumentCommand request,
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
            document.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            document,
            cancellationToken);
    }
}
