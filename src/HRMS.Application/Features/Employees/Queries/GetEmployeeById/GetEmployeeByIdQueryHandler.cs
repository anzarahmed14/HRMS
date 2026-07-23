using AutoMapper;
using HRMS.Application.Features.Employees.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler
    : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IReadRepository<Employee, Guid> _repository;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(
        IReadRepository<Employee, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto?> Handle(
        GetEmployeeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (employee is null)
            return null;

        return _mapper.Map<EmployeeDto>(employee);
    }
}