using HRMS.Application.Features.Employees.BusinessRules;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IReadRepository<Employee, Guid> _employeeReadRepository;
    private readonly IWriteRepository<Employee, Guid> _employeeWriteRepository;
    private readonly EmployeeBusinessRules _employeeRules;

    public DeleteEmployeeCommandHandler(
        IReadRepository<Employee, Guid> employeeReadRepository,
        IWriteRepository<Employee, Guid> employeeWriteRepository,
        EmployeeBusinessRules employeeRules)
    {
        _employeeReadRepository = employeeReadRepository;
        _employeeWriteRepository = employeeWriteRepository;
        _employeeRules = employeeRules;
    }

    public async Task Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Make sure employee exists
        await _employeeRules.EnsureEmployeeExistsAsync(
            request.Id,
            cancellationToken);

        // 2. Get employee
        var employee = await _employeeReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employee is null)
        {
            throw new InvalidOperationException(
                "Employee could not be loaded.");
        }

        // 3. Delete employee
        await _employeeWriteRepository.DeleteAsync(
            employee,
            cancellationToken);
    }
}