using AutoMapper;
using HRMS.Application.Features.Employees.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IReadRepository<Employee, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler( IReadRepository<Employee, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle( GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}