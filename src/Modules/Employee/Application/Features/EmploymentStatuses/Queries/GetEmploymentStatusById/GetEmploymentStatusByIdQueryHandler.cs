using AutoMapper;
using HRMS.Application.Features.EmploymentStatuses.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Queries.GetEmploymentStatusById;

public class GetEmploymentStatusByIdQueryHandler
    : IRequestHandler<GetEmploymentStatusByIdQuery, EmploymentStatusDto?>
{
    private readonly IReadRepository<EmploymentStatus, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmploymentStatusByIdQueryHandler(
        IReadRepository<EmploymentStatus, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmploymentStatusDto?> Handle(
        GetEmploymentStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employmentStatus = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employmentStatus is null)
            return null;

        return _mapper.Map<EmploymentStatusDto>(
            employmentStatus);
    }
}
