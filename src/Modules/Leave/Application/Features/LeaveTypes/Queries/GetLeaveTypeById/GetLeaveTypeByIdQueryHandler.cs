using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;

public class GetLeaveTypeByIdQueryHandler
    : IRequestHandler<GetLeaveTypeByIdQuery, LeaveTypeDto>
{
    private readonly IReadRepository<LeaveType, Guid> _repository;

    public GetLeaveTypeByIdQueryHandler(
        IReadRepository<LeaveType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeaveTypeDto> Handle(
        GetLeaveTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Type",
                request.Id);
        }

        return new LeaveTypeDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsPaid = entity.IsPaid,
            IsActive = entity.IsActive
        };
    }
}
