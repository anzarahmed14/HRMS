using AutoMapper;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests;

public sealed class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDto>();
    }
}
