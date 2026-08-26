using AutoMapper;
using HRMS.Application.Features.EmploymentTypes.Commands.CreateEmploymentType;
using HRMS.Application.Features.EmploymentTypes.Commands.UpdateEmploymentType;
using HRMS.Application.Features.EmploymentTypes.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmploymentTypes.Mappings;

public class EmploymentTypeMappingProfile : Profile
{
    public EmploymentTypeMappingProfile()
    {
        CreateMap<CreateEmploymentTypeCommand, EmploymentType>();

        CreateMap<UpdateEmploymentTypeCommand, EmploymentType>();

        CreateMap<EmploymentType, EmploymentTypeDto>();
    }
}
