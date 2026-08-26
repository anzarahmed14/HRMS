using AutoMapper;
using HRMS.Application.Features.EmploymentStatuses.Commands.CreateEmploymentStatus;
using HRMS.Application.Features.EmploymentStatuses.Commands.UpdateEmploymentStatus;
using HRMS.Application.Features.EmploymentStatuses.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.EmploymentStatuses.Mappings;

public class EmploymentStatusProfile : Profile
{
    public EmploymentStatusProfile()
    {
        CreateMap<CreateEmploymentStatusCommand, EmploymentStatus>();

        CreateMap<UpdateEmploymentStatusCommand, EmploymentStatus>();

        CreateMap<EmploymentStatus, EmploymentStatusDto>();
    }
}
