using AutoMapper;
using HRMS.Application.Features.EmploymentTypes.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Queries.GetEmploymentTypeById;

public class GetEmploymentTypeByIdQueryHandler
    : IRequestHandler<GetEmploymentTypeByIdQuery, EmploymentTypeDto?>
{
    private readonly IReadRepository<EmploymentType, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmploymentTypeByIdQueryHandler(
        IReadRepository<EmploymentType, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmploymentTypeDto?> Handle(
        GetEmploymentTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employmentType = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employmentType is null)
            return null;

        return _mapper.Map<EmploymentTypeDto>(employmentType);
    }
}
