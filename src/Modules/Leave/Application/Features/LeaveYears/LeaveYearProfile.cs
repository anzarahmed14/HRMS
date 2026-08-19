using AutoMapper;
using HRMS.Modules.Leave.Application.Features.LeaveYears.DTOs;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears;

public class LeaveYearProfile : Profile
{
    public LeaveYearProfile()
    {
        CreateMap<LeaveYear, LeaveYearDto>();
    }
}