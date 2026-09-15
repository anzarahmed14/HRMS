using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatuses;

public class GetLeaveRequestStatusesQueryHandler
    : IRequestHandler<
        GetLeaveRequestStatusesQuery,
        PagedResult<LeaveRequestStatusDto>>
{
    private readonly IReadRepository<LeaveRequestStatus, Guid> _repository;

    public GetLeaveRequestStatusesQueryHandler(
        IReadRepository<LeaveRequestStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeaveRequestStatusDto>> Handle(
        GetLeaveRequestStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeaveRequestStatusDto>
        {
            Items = result.Items
                .Select(x => new LeaveRequestStatusDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
