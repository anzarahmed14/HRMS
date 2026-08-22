using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Queries.GetAttendancePolicyById;

public sealed record GetAttendancePolicyByIdQuery(
    Guid Id
) : IRequest<AttendancePolicyDto>;
