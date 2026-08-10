using AutoMapper;
using HRMS.Application.Features.Departments.BusinessRules;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IWriteRepository<Department, Guid> _writeRepository;
    private readonly DepartmentBusinessRules _departmentRules;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(
        IWriteRepository<Department, Guid> writeRepository,
        DepartmentBusinessRules departmentRules,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _departmentRules = departmentRules;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // Business Rule
        await _departmentRules.EnsureDepartmentNameUniqueAsync(
            request.Name,
            cancellationToken);

        // Map
        var department = _mapper.Map<Department>(request);

        // Save
        await _writeRepository.AddAsync(
            department,
            cancellationToken);

        return department.Id;
    }
}