using AutoMapper;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequests;

public sealed class GetLeaveRequestsQueryHandler
    : IRequestHandler<GetLeaveRequestsQuery, PagedResult<LeaveRequestDto>>
{
    private readonly IReadRepository<LeaveRequest, Guid> _repository;
    private readonly IMapper _mapper;

    public GetLeaveRequestsQueryHandler(
        IReadRepository<LeaveRequest, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<LeaveRequestDto>> Handle(
        GetLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request,
            cancellationToken: cancellationToken);

        return new PagedResult<LeaveRequestDto>
        {
            Items = _mapper.Map<List<LeaveRequestDto>>(result.Items),
            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
