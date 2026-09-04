using AutoMapper;
using HRMS.Modules.Companies.Application.Features.Companies.Commands.CreateCompany;
using HRMS.Modules.Companies.Application.Features.Companies.Commands.UpdateCompany;
using HRMS.Modules.Companies.Domain.Entities;

namespace HRMS.Modules.Companies.Application.Features.Companies.Mappings;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyCommand, Company>();

        CreateMap<UpdateCompanyCommand, Company>()
            .ForMember(x => x.Id, opt => opt.Ignore());
    }
}
