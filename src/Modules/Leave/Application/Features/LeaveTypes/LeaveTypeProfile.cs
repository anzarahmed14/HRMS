using AutoMapper;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.DTOs;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes;

public class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveType, LeaveTypeDto>();
    }
}