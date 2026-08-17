using AutoMapper;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Department.Application.Features.Departments.BusinessRules;
using HRMS.Modules.Department.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IReadRepository<Department, Guid> _departmentReadRepository;
    private readonly IWriteRepository<Department, Guid> _departmentWriteRepository;
    private readonly DepartmentBusinessRules _departmentRules;
    private readonly IMapper _mapper;

    public UpdateDepartmentCommandHandler(
        IReadRepository<Department, Guid> departmentReadRepository,
        IWriteRepository<Department, Guid> departmentWriteRepository,
        DepartmentBusinessRules departmentRules,
        IMapper mapper)
    {
        _departmentReadRepository = departmentReadRepository;
        _departmentWriteRepository = departmentWriteRepository;
        _departmentRules = departmentRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Department must exist
        await _departmentRules.EnsureDepartmentExistsAsync(
            request.Id,
            cancellationToken);

        // 2. Department name must be unique
        //    Excludes the current department.
        await _departmentRules.EnsureDepartmentNameUniqueAsync(
            request.Name,
            request.Id,
            cancellationToken);

        // 3. Get existing department
        var department = await _departmentReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (department is null)
        {
            throw new InvalidOperationException(
                "Department could not be loaded.");
        }

        // 4. Map request to existing entity
        _mapper.Map(request, department);

        // 5. Save changes
        await _departmentWriteRepository.UpdateAsync(
            department,
            cancellationToken);
    }
}