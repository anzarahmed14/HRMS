using AutoMapper;
using HRMS.Application.Features.Departments.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Department.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Department.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto?>
{
    private readonly IReadRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> _repository;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(
        IReadRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DepartmentDto?> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (department is null)
            return null;

        return _mapper.Map<DepartmentDto>(department);
    }
}