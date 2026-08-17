using AutoMapper;
using HRMS.Application.Features.Departments.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Department.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Department.Application.Features.Departments.Queries.GetDepartments;

public class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IReadRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> _repository;
    private readonly IMapper _mapper;

    public GetDepartmentsQueryHandler(
        IReadRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await _repository.GetAllAsync(
            cancellationToken);

        return _mapper.Map<IReadOnlyList<DepartmentDto>>(
            departments);
    }
}