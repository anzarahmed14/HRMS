using AutoMapper;
using HRMS.Application.Features.Experiences.Commands.CreateEmployeeExperience;
using HRMS.Application.Features.Experiences.Commands.UpdateEmployeeExperience;
using HRMS.Application.Features.Experiences.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Experiences.Mappings;

public class EmployeeExperienceProfile : Profile
{
    public EmployeeExperienceProfile()
    {
        CreateMap<CreateEmployeeExperienceCommand, EmployeeExperience>();

        CreateMap<UpdateEmployeeExperienceCommand, EmployeeExperience>();

        CreateMap<EmployeeExperience, EmployeeExperienceDto>();
    }
}
