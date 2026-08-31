using AutoMapper;
using HRMS.Application.Features.Nominees.Commands.CreateEmployeeNominee;
using HRMS.Application.Features.Nominees.Commands.UpdateEmployeeNominee;
using HRMS.Application.Features.Nominees.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Nominees.Mappings;

public class EmployeeNomineeProfile : Profile
{
    public EmployeeNomineeProfile()
    {
        CreateMap<CreateEmployeeNomineeCommand, EmployeeNominee>();

        CreateMap<UpdateEmployeeNomineeCommand, EmployeeNominee>();

        CreateMap<EmployeeNominee, EmployeeNomineeDto>();
    }
}
