using AutoMapper;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.BusinessRules;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.CreateAttendancePolicy;

public class CreateAttendancePolicyCommandHandler
    : IRequestHandler<CreateAttendancePolicyCommand, Guid>
{
    private readonly IWriteRepository<AttendancePolicy, Guid>
        _attendancePolicyWriteRepository;

    private readonly AttendancePolicyBusinessRules
        _attendancePolicyRules;

    private readonly IMapper _mapper;

    public CreateAttendancePolicyCommandHandler(
        IWriteRepository<AttendancePolicy, Guid> attendancePolicyWriteRepository,
        AttendancePolicyBusinessRules attendancePolicyRules,
        IMapper mapper)
    {
        _attendancePolicyWriteRepository = attendancePolicyWriteRepository;
        _attendancePolicyRules = attendancePolicyRules;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateAttendancePolicyCommand request,
        CancellationToken cancellationToken)
    {
        await _attendancePolicyRules.EnsureCompanyExistsAsync(
            request.CompanyId,
            cancellationToken);

        await _attendancePolicyRules.EnsurePolicyCodeUniqueAsync(
            request.CompanyId,
            request.Code,
            cancellationToken: cancellationToken);

        if (request.IsDefault)
        {
            await _attendancePolicyRules.EnsureDefaultPolicyUniqueAsync(
                request.CompanyId,
                cancellationToken: cancellationToken);
        }

        var attendancePolicy =
            _mapper.Map<AttendancePolicy>(request);

        await _attendancePolicyWriteRepository.AddAsync(
            attendancePolicy,
            cancellationToken);

        return attendancePolicy.Id;
    }
}