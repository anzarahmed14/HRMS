using AutoMapper;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.DTOs;
using HRMS.Modules.Leave.Domain.Entities;

namespace HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements;

public class EmployeeLeaveEntitlementProfile : Profile
{
    public EmployeeLeaveEntitlementProfile()
    {
        CreateMap<EmployeeLeaveEntitlement, EmployeeLeaveEntitlementDto>();
    }
}