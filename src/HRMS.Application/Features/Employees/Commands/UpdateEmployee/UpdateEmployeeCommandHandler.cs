using AutoMapper;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IReadRepository<Employee, Guid> _employeeReadRepository;
    private readonly IWriteRepository<Employee, Guid> _employeeWriteRepository;
    private readonly IReadRepository<Department, Guid> _departmentReadRepository;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(
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

    public async Task Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employee is null)
            throw new Exception("Employee not found.");

        var department = await _departmentReadRepository.GetByIdAsync(
            request.DepartmentId,
            cancellationToken);

        if (department is null)
            throw new Exception("Department not found.");

        var employeeCodeExists = await _employeeReadRepository.AnyAsync(
            x => x.EmployeeCode == request.EmployeeCode &&
                 x.Id != request.Id,
            cancellationToken);

        if (employeeCodeExists)
            throw new Exception("Employee code already exists.");

        var emailExists = await _employeeReadRepository.AnyAsync(
            x => x.Email == request.Email &&
                 x.Id != request.Id,
            cancellationToken);

        if (emailExists)
            throw new Exception("Email already exists.");

        _mapper.Map(request, employee);

        await _employeeWriteRepository.UpdateAsync(
            employee,
            cancellationToken);
    }
}