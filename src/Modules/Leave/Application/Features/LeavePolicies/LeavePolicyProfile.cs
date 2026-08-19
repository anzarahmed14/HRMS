using AutoMapper;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.DTOs;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicies;

public class LeavePolicyProfile : Profile
{
    public LeavePolicyProfile()
    {
        CreateMap<LeavePolicy, LeavePolicyDto>();
    }
}