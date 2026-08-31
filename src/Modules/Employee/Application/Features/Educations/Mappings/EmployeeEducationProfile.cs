using AutoMapper;
using HRMS.Application.Features.Educations.Commands.CreateEmployeeEducation;
using HRMS.Application.Features.Educations.Commands.UpdateEmployeeEducation;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Educations.Mappings;

public class EmployeeEducationProfile : Profile
{
    public EmployeeEducationProfile()
    {
        CreateMap<CreateEmployeeEducationCommand, EmployeeEducation>();

        CreateMap<UpdateEmployeeEducationCommand, EmployeeEducation>();
    }
}

