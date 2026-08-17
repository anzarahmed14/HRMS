
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using MediatR;
using HRMS.Modules.Department.Application.Features.Departments.Commands.DeleteDepartment;
using HRMS.Modules.Department.Domain.Entities;
using HRMS.Modules.Department.Application.Features.Departments.BusinessRules;


namespace HRMS.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler
    : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IReadRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> _departmentReadRepository;
    private readonly IWriteRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> _departmentWriteRepository;
    private readonly DepartmentBusinessRules _departmentRules;

    public DeleteDepartmentCommandHandler(
        IReadRepository<HRMS.Modules.Department.Domain.Entities.Department, Guid> departmentReadRepository,
        IWriteRepository<HRMS.Modules.Department.Domain.Entities.       Department, Guid> departmentWriteRepository,
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