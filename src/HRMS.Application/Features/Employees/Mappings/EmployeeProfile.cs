using AutoMapper;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Commands.UpdateEmployee;
using HRMS.Application.Features.Employees.DTOs;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Employees.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        // Entity -> DTO
        CreateMap<Employee, EmployeeDto>()
            .ForMember(
                dest => dest.DepartmentName,
                opt => opt.MapFrom(src =>
                    src.Department != null ? src.Department.Name : null));

        // Create DTO -> Entity
       CreateMap<CreateEmployeeCommand, Employee>();

        // Update DTO -> Entity
        CreateMap<UpdateEmployeeCommand, Employee>();
    }
}