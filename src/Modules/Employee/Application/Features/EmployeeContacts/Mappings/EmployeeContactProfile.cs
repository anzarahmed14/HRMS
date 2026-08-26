using AutoMapper;
using HRMS.Application.Features.EmployeeContacts.Commands.CreateEmployeeContact;
using HRMS.Application.Features.EmployeeContacts.Commands.UpdateEmployeeContact;
using HRMS.Application.Features.EmployeeContacts.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmployeeContacts.Mappings;

public class EmployeeContactProfile : Profile
{
    public EmployeeContactProfile()
    {
        CreateMap<CreateEmployeeContactCommand, EmployeeContact>();

        CreateMap<UpdateEmployeeContactCommand, EmployeeContact>();

        CreateMap<EmployeeContact, EmployeeContactDto>();
    }
}
