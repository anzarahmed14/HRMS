using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.DeleteAttendancePolicy;

public sealed class DeleteAttendancePolicyCommandHandler
    : IRequestHandler<DeleteAttendancePolicyCommand>
{
    private readonly IReadRepository<AttendancePolicy, Guid> _readRepository;
    private readonly IWriteRepository<AttendancePolicy, Guid> _writeRepository;

    public DeleteAttendancePolicyCommandHandler(
        IReadRepository<AttendancePolicy, Guid> readRepository,
        IWriteRepository<AttendancePolicy, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        DeleteAttendancePolicyCommand request,
        CancellationToken cancellationToken)
    {
        var policy = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (policy is null || policy.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance Policy",
                request.Id);
        }

        await _writeRepository.DeleteAsync(
            policy,
            cancellationToken);
    }
}
