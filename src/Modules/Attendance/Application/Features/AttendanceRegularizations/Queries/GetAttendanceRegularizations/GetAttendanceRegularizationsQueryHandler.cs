using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.DTOs;
using HRMS.Modules.Attendance.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizations;

public sealed class GetAttendanceRegularizationsQueryHandler
    : IRequestHandler<
        GetAttendanceRegularizationsQuery,
        PagedResult<AttendanceRegularizationDto>>
{
    private readonly IReadRepository<AttendanceRegularization, Guid>
        _regularizationRepository;

    private readonly IReadRepository<AttendanceRegularizationStatus, Guid>
        _statusRepository;

    public GetAttendanceRegularizationsQueryHandler(
        IReadRepository<AttendanceRegularization, Guid> regularizationRepository,
        IReadRepository<AttendanceRegularizationStatus, Guid> statusRepository)
    {
        _regularizationRepository = regularizationRepository;
        _statusRepository = statusRepository;
    }

    public async Task<PagedResult<AttendanceRegularizationDto>> Handle(
        GetAttendanceRegularizationsQuery request,
        CancellationToken cancellationToken)
    {
        var pagedRequest = new PagedRequest
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var result =
            await _regularizationRepository.GetPagedAsync(
                pagedRequest,
                x =>
                    !x.IsDeleted &&
                    (!request.EmployeeId.HasValue ||
                     x.EmployeeId == request.EmployeeId.Value) &&
                    (!request.FromDate.HasValue ||
                     x.AttendanceDate >= request.FromDate.Value) &&
                    (!request.ToDate.HasValue ||
                     x.AttendanceDate <= request.ToDate.Value) &&
                    (!request.StatusId.HasValue ||
                     x.AttendanceRegularizationStatusId ==
                        request.StatusId.Value),
                x => x.OrderByDescending(y => y.AttendanceDate),
                cancellationToken);

        var statusIds = result.Items
            .Select(x => x.AttendanceRegularizationStatusId)
            .Distinct()
            .ToList();

        var statuses = await _statusRepository.FindAsync(
            x =>
                statusIds.Contains(x.Id) &&
                !x.IsDeleted,
            cancellationToken);

        var statusLookup = statuses.ToDictionary(
            x => x.Id);

        var items = result.Items
            .Select(x =>
            {
                statusLookup.TryGetValue(
                    x.AttendanceRegularizationStatusId,
                    out var status);

                return new AttendanceRegularizationDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    AttendanceRecordId = x.AttendanceRecordId,
                    AttendanceDate = x.AttendanceDate,
                    RequestedCheckIn = x.RequestedCheckIn,
                    RequestedCheckOut = x.RequestedCheckOut,
                    Reason = x.Reason,

                    AttendanceRegularizationStatusId =
                        x.AttendanceRegularizationStatusId,

                    StatusCode =
                        status?.Code ?? string.Empty,

                    StatusName =
                        status?.Name ?? string.Empty,

                    RequestedBy = x.RequestedBy,
                    RequestedOn = x.RequestedOn,

                    ApprovedBy = x.ApprovedBy,
                    ApprovedOn = x.ApprovedOn,
                    ApprovalRemarks = x.ApprovalRemarks,

                    RejectedBy = x.RejectedBy,
                    RejectedOn = x.RejectedOn,
                    RejectionRemarks = x.RejectionRemarks,

                    CancelledBy = x.CancelledBy,
                    CancelledOn = x.CancelledOn,
                    CancellationRemarks =
                        x.CancellationRemarks
                };
            })
            .ToList();

        return new PagedResult<AttendanceRegularizationDto>
        {
            Items = items,
            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
