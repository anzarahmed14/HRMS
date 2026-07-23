using AutoMapper;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IReadRepository<Employee, Guid> _employeeReadRepository;
    private readonly IWriteRepository<Employee, Guid> _employeeWriteRepository;
    private readonly IReadRepository<Department, Guid> _departmentReadRepository;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(
        IReadRepository<Employee, Guid> employeeReadRepository,
        IWriteRepository<Employee, Guid> employeeWriteRepository,
        IReadRepository<Department, Guid> departmentReadRepository,
        IMapper mapper)
    {
        _employeeReadRepository = employeeReadRepository;
        _employeeWriteRepository = employeeWriteRepository;
        _departmentReadRepository = departmentReadRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        // Check Employee Code
        if (await _employeeReadRepository.AnyAsync(
                x => x.EmployeeCode == request.EmployeeCode,
                cancellationToken))
        {
            throw new Exception($"Employee Code '{request.EmployeeCode}' already exists.");
        }

        // Check Email
        if (await _employeeReadRepository.AnyAsync(
                x => x.Email == request.Email,
                cancellationToken))
        {
            throw new Exception($"Email '{request.Email}' already exists.");
        }

        // Check Department
        var department = await _departmentReadRepository.GetByIdAsync(
            request.DepartmentId,
            cancellationToken);

        if (department is null)
        {
            throw new Exception("Department not found.");
        }

        // Mapping
        var employee = _mapper.Map<Employee>(request);

        await _employeeWriteRepository.AddAsync(employee, cancellationToken);

        return employee.Id;
    }
}