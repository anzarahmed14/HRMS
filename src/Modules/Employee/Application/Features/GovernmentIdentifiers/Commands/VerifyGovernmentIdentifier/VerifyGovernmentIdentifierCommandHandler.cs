using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.VerifyGovernmentIdentifier;

public class VerifyGovernmentIdentifierCommandHandler
    : IRequestHandler<VerifyGovernmentIdentifierCommand>
{
    private readonly IReadRepository<EmployeeGovernmentIdentifier, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeGovernmentIdentifier, Guid> _writeRepository;

    public VerifyGovernmentIdentifierCommandHandler(
        IReadRepository<EmployeeGovernmentIdentifier, Guid> readRepository,
        IWriteRepository<EmployeeGovernmentIdentifier, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        VerifyGovernmentIdentifierCommand request,
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

        identifier.IsVerified = true;
        identifier.VerifiedOn = DateTimeOffset.UtcNow;

        await _writeRepository.UpdateAsync(
            identifier,
            cancellationToken);
    }
}
