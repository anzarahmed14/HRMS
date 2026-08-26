using HRMS.Application.Features.EmergencyContacts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Commands.CreateEmergencyContact;

public class CreateEmergencyContactCommandHandler
    : IRequestHandler<CreateEmergencyContactCommand, Guid>
{
    private readonly IWriteRepository<EmergencyContact, Guid> _writeRepository;
    private readonly EmergencyContactBusinessRules _businessRules;

    public CreateEmergencyContactCommandHandler(
        IWriteRepository<EmergencyContact, Guid> writeRepository,
        EmergencyContactBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmergencyContactCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureRelationshipExistsAsync(
            request.RelationshipId,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryContactAvailableAsync(
                request.EmployeeId,
                cancellationToken);
        }

        var emergencyContact = new EmergencyContact
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            Name = request.Name,
            RelationshipId = request.RelationshipId,
            PhoneNumber = request.PhoneNumber,
            AlternatePhoneNumber = request.AlternatePhoneNumber,
            Email = request.Email,
            IsPrimary = request.IsPrimary
        };

        await _writeRepository.AddAsync(
            emergencyContact,
            cancellationToken);

        return emergencyContact.Id;
    }
}
