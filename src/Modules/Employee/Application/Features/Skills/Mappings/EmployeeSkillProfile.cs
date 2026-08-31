using AutoMapper;
using HRMS.Application.Features.Skills.Commands.CreateEmployeeSkill;
using HRMS.Application.Features.Skills.Commands.UpdateEmployeeSkill;
using HRMS.Application.Features.Skills.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Skills.Mappings;

public class EmployeeSkillProfile : Profile
{
    public EmployeeSkillProfile()
    {
        CreateMap<CreateEmployeeSkillCommand, EmployeeSkill>();

        CreateMap<UpdateEmployeeSkillCommand, EmployeeSkill>();

        CreateMap<EmployeeSkill, EmployeeSkillDto>();
    }
}
