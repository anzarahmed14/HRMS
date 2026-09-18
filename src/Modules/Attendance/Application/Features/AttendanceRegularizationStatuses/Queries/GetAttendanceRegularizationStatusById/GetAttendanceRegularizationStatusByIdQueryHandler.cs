using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Queries.GetAttendanceRegularizationStatusById;

public sealed class GetAttendanceRegularizationStatusByIdQueryHandler
    : IRequestHandler<GetAttendanceRegularizationStatusByIdQuery, AttendanceRegularizationStatusDto>
{
    private readonly AttendanceRegularizationStatusBusinessRules _businessRules;

    public GetAttendanceRegularizationStatusByIdQueryHandler(
        AttendanceRegularizationStatusBusinessRules businessRules)
    {
        _businessRules = businessRules;
    }

    public async Task<AttendanceRegularizationStatusDto> Handle(
        GetAttendanceRegularizationStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var status = await _businessRules.EnsureExistsAsync(
            request.Id,
            cancellationToken);

        return new AttendanceRegularizationStatusDto
        {
            Id = status.Id,
            Code = status.Code,
            Name = status.Name,
            IsActive = status.IsActive
        };
    }
}
