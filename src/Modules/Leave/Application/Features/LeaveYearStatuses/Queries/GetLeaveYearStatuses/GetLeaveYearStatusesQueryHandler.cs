using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatuses;

public class GetLeaveYearStatusesQueryHandler
    : IRequestHandler<
        GetLeaveYearStatusesQuery,
        PagedResult<LeaveYearStatusListDto>>
{
    private readonly IReadRepository<LeaveYearStatus, Guid>
        _repository;

    public GetLeaveYearStatusesQueryHandler(
        IReadRepository<LeaveYearStatus, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeaveYearStatusListDto>> Handle(
        GetLeaveYearStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeaveYearStatusListDto>
        {
            Items = result.Items
                .Select(x => new LeaveYearStatusListDto
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
