using AutoMapper;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.DTOs;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeavePolicyRules;

public class LeavePolicyRuleProfile : Profile
{
    public LeavePolicyRuleProfile()
    {
        CreateMap<LeavePolicyRule, LeavePolicyRuleDto>();
    }
}