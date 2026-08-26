using AutoMapper;
using HRMS.Application.Features.EmergencyContacts.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Commands.UpdateEmergencyContact;

public class UpdateEmergencyContactCommandHandler
    : IRequestHandler<UpdateEmergencyContactCommand>
{
    private readonly IReadRepository<EmergencyContact, Guid> _readRepository;
    private readonly IWriteRepository<EmergencyContact, Guid> _writeRepository;
    private readonly EmergencyContactBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmergencyContactCommandHandler(
        IReadRepository<EmergencyContact, Guid> readRepository,
        IWriteRepository<EmergencyContact, Guid> writeRepository,
        EmergencyContactBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmergencyContactCommand request,
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
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureRelationshipExistsAsync(
            request.RelationshipId,
            cancellationToken);

        if (request.IsPrimary)
        {
            await _businessRules.EnsurePrimaryContactAvailableAsync(
                request.EmployeeId,
                request.Id,
                cancellationToken);
        }

        _mapper.Map(request, emergencyContact);

        await _writeRepository.UpdateAsync(
            emergencyContact,
            cancellationToken);
    }
}
