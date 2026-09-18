using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Queries.GetAttendanceDayStatusById;

public sealed class GetAttendanceDayStatusByIdQueryHandler
    : IRequestHandler<GetAttendanceDayStatusByIdQuery, AttendanceDayStatusDto>
{
    private readonly AttendanceDayStatusBusinessRules _businessRules;

    public GetAttendanceDayStatusByIdQueryHandler(
        AttendanceDayStatusBusinessRules businessRules)
    {
        _businessRules = businessRules;
    }

    public async Task<AttendanceDayStatusDto> Handle(
        GetAttendanceDayStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dayStatus = await _businessRules.EnsureExistsAsync(
            request.Id,
            cancellationToken);

        return new AttendanceDayStatusDto
        {
            Id = dayStatus.Id,
            Code = dayStatus.Code,
            Name = dayStatus.Name,
            Description = dayStatus.Description,
            IsActive = dayStatus.IsActive
        };
    }
}
