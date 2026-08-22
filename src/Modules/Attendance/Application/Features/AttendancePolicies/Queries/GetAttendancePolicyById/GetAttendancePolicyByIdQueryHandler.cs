using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Queries.GetAttendancePolicyById;

public sealed class GetAttendancePolicyByIdQueryHandler
    : IRequestHandler<GetAttendancePolicyByIdQuery, AttendancePolicyDto>
{
    private readonly IReadRepository<AttendancePolicy, Guid> _repository;

    public GetAttendancePolicyByIdQueryHandler(
        IReadRepository<AttendancePolicy, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AttendancePolicyDto> Handle(
        GetAttendancePolicyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var policy = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (policy is null || policy.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Policy",
                request.Id);
        }

        return new AttendancePolicyDto
        {
            Id = policy.Id,
            CompanyId = policy.CompanyId,
            Code = policy.Code,
            Name = policy.Name,
            Description = policy.Description,
            GracePeriodMinutes = policy.GracePeriodMinutes,
            MinimumWorkingMinutes = policy.MinimumWorkingMinutes,
            FullDayMinutes = policy.FullDayMinutes,
            HalfDayMinutes = policy.HalfDayMinutes,
            IsOvertimeAllowed = policy.IsOvertimeAllowed,
            MinimumOvertimeMinutes = policy.MinimumOvertimeMinutes,
            MaximumOvertimeMinutes = policy.MaximumOvertimeMinutes,
            OvertimeRequiresApproval = policy.OvertimeRequiresApproval,
            IsDefault = policy.IsDefault,
            IsActive = policy.IsActive,
            EffectiveFrom = policy.EffectiveFrom,
            EffectiveTo = policy.EffectiveTo
        };
    }
}
