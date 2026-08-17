
using AutoMapper;
using HRMS.Application.Features.Employees.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using MediatR;

namespace HRMS.Modules.Employee.Application.Features.Employees.Queries.GetEmployees;
public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IReadRepository<HRMS.Modules.Employee.Domain.Entities.Employee, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler( IReadRepository<HRMS.Modules.Employee.Domain.Entities.Employee, Guid> repository, IMapper mapper)
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