using HRMS.Application.Features.Departments.BusinessRules;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler
    : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IReadRepository<Department, Guid> _departmentReadRepository;
    private readonly IWriteRepository<Department, Guid> _departmentWriteRepository;
    private readonly DepartmentBusinessRules _departmentRules;

    public DeleteDepartmentCommandHandler(
        IReadRepository<Department, Guid> departmentReadRepository,
        IWriteRepository<Department, Guid> departmentWriteRepository,
        DepartmentBusinessRules departmentRules)
    {
        _departmentReadRepository = departmentReadRepository;
        _departmentWriteRepository = departmentWriteRepository;
        _departmentRules = departmentRules;
    }

    public async Task Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Department must exist
        await _departmentRules.EnsureDepartmentExistsAsync(
            request.Id,
            cancellationToken);

        // 2. Get department
        var department = await _departmentReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (department is null)
        {
            throw new InvalidOperationException(
                "Department could not be loaded.");
        }

        // 3. Delete department
        await _departmentWriteRepository.DeleteAsync(
            department,
            cancellationToken);
    }
}