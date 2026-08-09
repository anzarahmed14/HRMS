using AutoMapper;
using HRMS.Application.Features.Employees.BusinessRules;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IReadRepository<Employee, Guid> _employeeReadRepository;
    private readonly IWriteRepository<Employee, Guid> _employeeWriteRepository;
    private readonly EmployeeBusinessRules _employeeRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(
        IReadRepository<Employee, Guid> employeeReadRepository,
        IWriteRepository<Employee, Guid> employeeWriteRepository,
        EmployeeBusinessRules employeeRules,
        IMapper mapper)
    {
        _employeeReadRepository = employeeReadRepository;
        _employeeWriteRepository = employeeWriteRepository;
        _employeeRules = employeeRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Employee must exist
        await _employeeRules.EnsureEmployeeExistsAsync(
            request.Id,
            cancellationToken);

        // 2. Department must exist
        await _employeeRules.EnsureDepartmentExistsAsync(
            request.DepartmentId,
            cancellationToken);

        // 3. Employee code must be unique
        await _employeeRules.EnsureEmployeeCodeUniqueAsync(
            request.EmployeeCode,
            request.Id,
            cancellationToken);

        // 4. Email must be unique
        await _employeeRules.EnsureEmailUniqueAsync(
            request.Email,
            request.Id,
            cancellationToken);

        // 5. Get employee
        var employee = await _employeeReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        // This should never be null because of EnsureEmployeeExistsAsync().
        // Keeping the check protects us from unexpected changes.
        if (employee is null)
        {
            throw new InvalidOperationException(
                "Employee could not be loaded.");
        }

        // 6. Map request → existing employee
        _mapper.Map(request, employee);

        // 7. Update database
        await _employeeWriteRepository.UpdateAsync(
            employee,
            cancellationToken);
    }
}