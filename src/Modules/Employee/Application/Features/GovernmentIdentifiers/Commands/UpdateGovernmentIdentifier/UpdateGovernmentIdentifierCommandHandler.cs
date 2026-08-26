using AutoMapper;
using HRMS.Application.Features.GovernmentIdentifiers.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.UpdateGovernmentIdentifier;

public class UpdateGovernmentIdentifierCommandHandler
    : IRequestHandler<UpdateGovernmentIdentifierCommand>
{
    private readonly IReadRepository<EmployeeGovernmentIdentifier, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeGovernmentIdentifier, Guid> _writeRepository;
    private readonly EmployeeGovernmentIdentifierBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateGovernmentIdentifierCommandHandler(
        IReadRepository<EmployeeGovernmentIdentifier, Guid> readRepository,
        IWriteRepository<EmployeeGovernmentIdentifier, Guid> writeRepository,
        EmployeeGovernmentIdentifierBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateGovernmentIdentifierCommand request,
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
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureIdentifierTypeExistsAsync(
            request.IdentifierTypeId,
            cancellationToken);

        await _businessRules.EnsureIdentifierTypeAvailableAsync(
            request.EmployeeId,
            request.IdentifierTypeId,
            request.Id,
            cancellationToken);

        _mapper.Map(request, identifier);

        // Verification is intentionally not changed by this command.
        // Existing IsVerified and VerifiedOn values are preserved.

        await _writeRepository.UpdateAsync(
            identifier,
            cancellationToken);
    }
}
