using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IReadRepository<Employee, Guid> _employeeReadRepository;
    private readonly IWriteRepository<Employee, Guid> _employeeWriteRepository;

    public DeleteEmployeeCommandHandler(
        IReadRepository<Employee, Guid> employeeReadRepository,
        IWriteRepository<Employee, Guid> employeeWriteRepository)
    {
        _employeeReadRepository = employeeReadRepository;
        _employeeWriteRepository = employeeWriteRepository;
    }

    public async Task Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employee is null)
            throw new Exception("Employee not found.");

        await _employeeWriteRepository.DeleteAsync(
            employee,
            cancellationToken);
    }
}