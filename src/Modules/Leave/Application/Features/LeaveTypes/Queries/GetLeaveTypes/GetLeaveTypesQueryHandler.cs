using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Queries.GetLeaveTypes;

public class GetLeaveTypesQueryHandler
    : IRequestHandler<GetLeaveTypesQuery, PagedResult<LeaveTypeListDto>>
{
    private readonly IReadRepository<LeaveType, Guid> _repository;

    public GetLeaveTypesQueryHandler(
        IReadRepository<LeaveType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LeaveTypeListDto>> Handle(
        GetLeaveTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeaveTypeListDto>
        {
            Items = result.Items
                .Select(x => new LeaveTypeListDto
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    IsPaid = x.IsPaid,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
