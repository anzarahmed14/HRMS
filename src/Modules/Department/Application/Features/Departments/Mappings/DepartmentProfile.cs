using AutoMapper;
using HRMS.Application.Features.Departments.Commands.UpdateDepartment;
using HRMS.Application.Features.Departments.DTOs;
using HRMS.Modules.Department.Application.Features.Departments.Commands.CreateDepartment;
using HRMS.Modules.Department.Domain.Entities;


namespace HRMS.Application.Features.Departments.Mappings;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<CreateDepartmentCommand, Department>();

        CreateMap<UpdateDepartmentCommand, Department>();

        CreateMap<Department, DepartmentDto>();
    }
}