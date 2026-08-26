using HRMS.Application.Features.GovernmentIdentifiers.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.CreateGovernmentIdentifier;

public class CreateGovernmentIdentifierCommandHandler
    : IRequestHandler<CreateGovernmentIdentifierCommand, Guid>
{
    private readonly IWriteRepository<EmployeeGovernmentIdentifier, Guid> _writeRepository;
    private readonly EmployeeGovernmentIdentifierBusinessRules _businessRules;

    public CreateGovernmentIdentifierCommandHandler(
        IWriteRepository<EmployeeGovernmentIdentifier, Guid> writeRepository,
        EmployeeGovernmentIdentifierBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateGovernmentIdentifierCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureIdentifierTypeExistsAsync(
            request.IdentifierTypeId,
            cancellationToken);

        await _businessRules.EnsureIdentifierTypeAvailableAsync(
            request.EmployeeId,
            request.IdentifierTypeId,
            cancellationToken);

        var identifier = new EmployeeGovernmentIdentifier
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            IdentifierTypeId = request.IdentifierTypeId,
            IdentifierNumber = request.IdentifierNumber,
            IssueDate = request.IssueDate,
            ExpiryDate = request.ExpiryDate,
            IsVerified = false,
            VerifiedOn = null
        };

        await _writeRepository.AddAsync(
            identifier,
            cancellationToken);

        return identifier.Id;
    }
}
