using AutoMapper;
using HRMS.Application.Features.Departments.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Departments.Queries.GetDepartments;

public class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IReadRepository<Department, Guid> _repository;
    private readonly IMapper _mapper;

    public GetDepartmentsQueryHandler(
        IReadRepository<Department, Guid> repository,
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