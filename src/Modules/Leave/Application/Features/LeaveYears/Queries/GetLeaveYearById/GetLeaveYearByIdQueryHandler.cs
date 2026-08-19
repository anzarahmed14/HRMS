using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Leave.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveYears.Queries.GetLeaveYearById;

public class GetLeaveYearByIdQueryHandler
    : IRequestHandler<GetLeaveYearByIdQuery, LeaveYearDto>
{
    private readonly IReadRepository<LeaveYear, Guid> _repository;

    public GetLeaveYearByIdQueryHandler(
        IReadRepository<LeaveYear, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LeaveYearDto> Handle(
        GetLeaveYearByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Leave Year",
                request.Id);
        }

        return new LeaveYearDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            Code = entity.Code,
            Name = entity.Name,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            StatusId = entity.StatusId
        };
    }
}
