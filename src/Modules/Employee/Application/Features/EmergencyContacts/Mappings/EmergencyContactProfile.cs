using AutoMapper;
using HRMS.Application.Features.EmergencyContacts.Commands.CreateEmergencyContact;
using HRMS.Application.Features.EmergencyContacts.Commands.UpdateEmergencyContact;
using HRMS.Application.Features.EmergencyContacts.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmergencyContacts.Mappings;

public class EmergencyContactProfile : Profile
{
    public EmergencyContactProfile()
    {
        CreateMap<CreateEmergencyContactCommand, EmergencyContact>();

        CreateMap<UpdateEmergencyContactCommand, EmergencyContact>();

        CreateMap<EmergencyContact, EmergencyContactDto>();
    }
}
