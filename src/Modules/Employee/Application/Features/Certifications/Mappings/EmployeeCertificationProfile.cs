using AutoMapper;
using HRMS.Application.Features.Certifications.Commands.CreateEmployeeCertification;
using HRMS.Application.Features.Certifications.Commands.UpdateEmployeeCertification;
using HRMS.Application.Features.Certifications.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Certifications.Mappings;

public class EmployeeCertificationProfile : Profile
{
    public EmployeeCertificationProfile()
    {
        CreateMap<CreateEmployeeCertificationCommand, EmployeeCertification>();

        CreateMap<UpdateEmployeeCertificationCommand, EmployeeCertification>();

        CreateMap<EmployeeCertification, EmployeeCertificationDto>();
    }
}
