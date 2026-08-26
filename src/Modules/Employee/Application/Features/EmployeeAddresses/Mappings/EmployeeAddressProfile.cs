using AutoMapper;
using HRMS.Application.Features.EmployeeAddresses.Commands.CreateEmployeeAddress;
using HRMS.Application.Features.EmployeeAddresses.Commands.UpdateEmployeeAddress;
using HRMS.Application.Features.EmployeeAddresses.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmployeeAddresses.Mappings;

public class EmployeeAddressProfile : Profile
{
    public EmployeeAddressProfile()
    {
        CreateMap<CreateEmployeeAddressCommand, EmployeeAddress>();

        CreateMap<UpdateEmployeeAddressCommand, EmployeeAddress>();

        CreateMap<EmployeeAddress, EmployeeAddressDto>();
    }
}
