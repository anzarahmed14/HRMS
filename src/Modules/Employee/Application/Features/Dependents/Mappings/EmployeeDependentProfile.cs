using AutoMapper;
using HRMS.Application.Features.Dependents.Commands.CreateEmployeeDependent;
using HRMS.Application.Features.Dependents.Commands.UpdateEmployeeDependent;
using HRMS.Application.Features.Dependents.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Dependents.Mappings;

public class EmployeeDependentProfile : Profile
{
    public EmployeeDependentProfile()
    {
        CreateMap<CreateEmployeeDependentCommand, EmployeeDependent>();

        CreateMap<UpdateEmployeeDependentCommand, EmployeeDependent>();

        CreateMap<EmployeeDependent, EmployeeDependentDto>();
    }
}
