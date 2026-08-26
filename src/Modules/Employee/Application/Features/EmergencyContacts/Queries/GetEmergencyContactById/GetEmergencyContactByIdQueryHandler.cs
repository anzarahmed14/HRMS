using AutoMapper;
using HRMS.Application.Features.EmergencyContacts.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmergencyContacts.Queries.GetEmergencyContactById;

public class GetEmergencyContactByIdQueryHandler
    : IRequestHandler<GetEmergencyContactByIdQuery, EmergencyContactDto?>
{
    private readonly IReadRepository<EmergencyContact, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmergencyContactByIdQueryHandler(
        IReadRepository<EmergencyContact, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmergencyContactDto?> Handle(
        GetEmergencyContactByIdQuery request,
        CancellationToken cancellationToken)
    {
        var emergencyContact = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (emergencyContact is null)
            return null;

        return _mapper.Map<EmergencyContactDto>(
            emergencyContact);
    }
}
