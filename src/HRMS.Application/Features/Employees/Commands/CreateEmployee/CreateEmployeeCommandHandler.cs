using AutoMapper;
using HRMS.Application.Features.Employees.BusinessRules;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using HRMS.Shared.Exceptions;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IReadRepository<Employee, Guid> _employeeReadRepository;
    private readonly IWriteRepository<Employee, Guid> _employeeWriteRepository;
    private readonly IReadRepository<Department, Guid> _departmentReadRepository;
    private readonly IMapper _mapper;
    private readonly EmployeeBusinessRules _employeeRules;

    public CreateEmployeeCommandHandler(
        IReadRepository<Employee, Guid> employeeReadRepository,
        IWriteRepository<Employee, Guid> employeeWriteRepository,
        IReadRepository<Department, Guid> departmentReadRepository,
        IMapper mapper,
        EmployeeBusinessRules employeeRules)
    {
        _employeeReadRepository = employeeReadRepository;
        _employeeWriteRepository = employeeWriteRepository;
        _departmentReadRepository = departmentReadRepository;
        _mapper = mapper;
        _employeeRules = employeeRules;
    }

    public async Task<Guid> Handle(
    CreateEmployeeCommand request,
    CancellationToken cancellationToken)
    {
        await _employeeRules.EnsureEmployeeCodeUniqueAsync(
            request.EmployeeCode,
            cancellationToken);

        await _employeeRules.EnsureEmailUniqueAsync(
            request.Email,
            cancellationToken);

        await _employeeRules.EnsureDepartmentExistsAsync(
            request.DepartmentId,
            cancellationToken);

        var employee = _mapper.Map<Employee>(request);

        await _employeeWriteRepository.AddAsync(
            employee,
            cancellationToken);

        return employee.Id;
    }
}