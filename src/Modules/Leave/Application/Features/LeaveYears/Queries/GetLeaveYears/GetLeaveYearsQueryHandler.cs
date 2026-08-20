using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Queries.GetLeaveYears;

public class GetLeaveYearsQueryHandler
    : IRequestHandler<GetLeaveYearsQuery, PagedResult<LeaveYearListDto>>
{
    private readonly IReadRepository<LeaveYear, Guid> _repository;

    public GetLeaveYearsQueryHandler(
        IReadRepository<LeaveYear, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeaveYearListDto>> Handle(
        GetLeaveYearsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeaveYearListDto>
        {
            Items = result.Items
                .Select(x => new LeaveYearListDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    StatusId = x.StatusId
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
