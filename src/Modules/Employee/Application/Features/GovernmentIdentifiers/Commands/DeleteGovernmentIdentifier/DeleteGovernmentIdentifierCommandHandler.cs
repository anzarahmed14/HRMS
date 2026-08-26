using HRMS.Application.Features.GovernmentIdentifiers.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.DeleteGovernmentIdentifier;

public class DeleteGovernmentIdentifierCommandHandler
    : IRequestHandler<DeleteGovernmentIdentifierCommand>
{
    private readonly IReadRepository<EmployeeGovernmentIdentifier, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeGovernmentIdentifier, Guid> _writeRepository;
    private readonly EmployeeGovernmentIdentifierBusinessRules _businessRules;

    public DeleteGovernmentIdentifierCommandHandler(
        IReadRepository<EmployeeGovernmentIdentifier, Guid> readRepository,
        IWriteRepository<EmployeeGovernmentIdentifier, Guid> writeRepository,
        EmployeeGovernmentIdentifierBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteGovernmentIdentifierCommand request,
        CancellationToken cancellationToken)
    {
        var identifier = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (identifier is null)
        {
            throw new InvalidOperationException(
                "Government identifier could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            identifier.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            identifier,
            cancellationToken);
    }
}
