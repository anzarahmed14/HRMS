using AutoMapper;
using HRMS.Application.Features.GovernmentIdentifiers.Commands.CreateGovernmentIdentifier;
using HRMS.Application.Features.GovernmentIdentifiers.Commands.UpdateGovernmentIdentifier;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.GovernmentIdentifiers.Mappings;

public class GovernmentIdentifierProfile : Profile
{
    public GovernmentIdentifierProfile()
    {
        CreateMap<CreateGovernmentIdentifierCommand,
            EmployeeGovernmentIdentifier>();

        CreateMap<UpdateGovernmentIdentifierCommand,
            EmployeeGovernmentIdentifier>();
    }
}
