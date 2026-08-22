using AutoMapper;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.CreateAttendancePolicy;
using HRMS.Modules.Attendance.Domain.Entities;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Mappings;

public class AttendancePolicyProfile : Profile
{
    public AttendancePolicyProfile()
    {
        CreateMap<CreateAttendancePolicyCommand, AttendancePolicy>();
    }
}


