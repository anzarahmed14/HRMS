using AutoMapper;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;

public sealed class GetLeaveRequestByIdQueryHandler
    : IRequestHandler<GetLeaveRequestByIdQuery, LeaveRequestDto>
{
    private readonly IReadRepository<LeaveRequest, Guid> _repository;
    private readonly IMapper _mapper;

    public GetLeaveRequestByIdQueryHandler(
        IReadRepository<LeaveRequest, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LeaveRequestDto> Handle(
        GetLeaveRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Request",
                request.Id);
        }

        return _mapper.Map<LeaveRequestDto>(entity);
    }
}
