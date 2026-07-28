using AutoMapper;
using HRMS.Application.Features.Departments.Commands.CreateDepartment;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Departments.Mappings;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<CreateDepartmentCommand, Department>();
    }
}