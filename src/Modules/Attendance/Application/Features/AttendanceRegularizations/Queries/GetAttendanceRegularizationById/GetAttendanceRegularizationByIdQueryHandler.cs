using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizationById;

public sealed class GetAttendanceRegularizationByIdQueryHandler
    : IRequestHandler<
        GetAttendanceRegularizationByIdQuery,
        AttendanceRegularizationDto>
{
    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IReadRepository<AttendanceRegularizationStatus, Guid>
        _statusRepository;

    public GetAttendanceRegularizationByIdQueryHandler(
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IReadRepository<AttendanceRegularizationStatus, Guid> statusRepository)
    {
        _regularizationRepository = regularizationRepository;
        _statusRepository = statusRepository;
    }

    public async Task<AttendanceRegularizationDto> Handle(
        GetAttendanceRegularizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var regularization =
            await _regularizationRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (regularization is null ||
            regularization.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance regularization",
                request.Id);
        }

        var status =
            await _statusRepository.GetByIdAsync(
                regularization.AttendanceRegularizationStatusId,
                cancellationToken);

        if (status is null ||
            status.IsDeleted)
        {
            throw new NotFoundException(
                "Attendance regularization status",
                regularization.AttendanceRegularizationStatusId);
        }

        return new AttendanceRegularizationDto
        {
            Id = regularization.Id,

            EmployeeId =
                regularization.EmployeeId,

            AttendanceRecordId =
                regularization.AttendanceRecordId,

            AttendanceDate =
                regularization.AttendanceDate,

            RequestedCheckIn =
                regularization.RequestedCheckIn,

            RequestedCheckOut =
                regularization.RequestedCheckOut,

            Reason =
                regularization.Reason,

            AttendanceRegularizationStatusId =
                regularization.AttendanceRegularizationStatusId,

            StatusCode =
                status.Code,

            StatusName =
                status.Name,

            RequestedBy =
                regularization.RequestedBy,

            RequestedOn =
                regularization.RequestedOn,

            ApprovedBy =
                regularization.ApprovedBy,

            ApprovedOn =
                regularization.ApprovedOn,

            ApprovalRemarks =
                regularization.ApprovalRemarks,

            RejectedBy =
                regularization.RejectedBy,

            RejectedOn =
                regularization.RejectedOn,

            RejectionRemarks =
                regularization.RejectionRemarks,

            CancelledBy =
                regularization.CancelledBy,

            CancelledOn =
                regularization.CancelledOn,

            CancellationRemarks =
                regularization.CancellationRemarks
        };
    }
}
