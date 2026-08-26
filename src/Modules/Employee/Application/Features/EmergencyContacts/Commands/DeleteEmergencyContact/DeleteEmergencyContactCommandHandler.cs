using HRMS.Application.Features.EmergencyContacts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Commands.DeleteEmergencyContact;

public class DeleteEmergencyContactCommandHandler
    : IRequestHandler<DeleteEmergencyContactCommand>
{
    private readonly IReadRepository<EmergencyContact, Guid> _readRepository;
    private readonly IWriteRepository<EmergencyContact, Guid> _writeRepository;
    private readonly EmergencyContactBusinessRules _businessRules;

    public DeleteEmergencyContactCommandHandler(
        IReadRepository<EmergencyContact, Guid> readRepository,
        IWriteRepository<EmergencyContact, Guid> writeRepository,
        EmergencyContactBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmergencyContactCommand request,
        CancellationToken cancellationToken)
    {
        var emergencyContact = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (emergencyContact is null)
        {
            throw new InvalidOperationException(
                "Emergency contact could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            emergencyContact.EmployeeId,
            cancellationToken);

        await _writeRepository.DeleteAsync(
            emergencyContact,
            cancellationToken);
    }
}
