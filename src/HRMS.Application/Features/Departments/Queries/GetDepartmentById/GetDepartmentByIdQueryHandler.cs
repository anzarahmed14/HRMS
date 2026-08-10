using AutoMapper;
using HRMS.Application.Features.Departments.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto?>
{
    private readonly IReadRepository<Department, Guid> _repository;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(
        IReadRepository<Department, Guid> repository,
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